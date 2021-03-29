using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenEvent.Data.Models.Category;
using OpenEvent.Data.Models.Event;
using OpenEvent.Data.Models.Popularity;
using OpenEvent.Web.Contexts;
using OpenEvent.Web.Hubs;

namespace OpenEvent.Web.Services
{
    // TODO: converting lists to dictionaries would increase performance

    /// <inheritdoc />
    public class PopularityService : IPopularityService
    {
        private readonly ILogger<PopularityService> Logger;
        private readonly IServiceProvider ServiceProvider;
        private readonly IMapper Mapper;
        private readonly IHubContext<PopularityHub> PopularityHubContext;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="mapper"></param>
        /// <param name="popularityHubContext"></param>
        public PopularityService(ILogger<PopularityService> logger, IServiceProvider serviceProvider, IMapper mapper,
            IHubContext<PopularityHub> popularityHubContext)
        {
            Logger = logger;
            ServiceProvider = serviceProvider;
            Mapper = mapper;
            PopularityHubContext = popularityHubContext;
        }

        /// <inheritdoc />
        public async Task<List<PopularEventViewModel>> GetPopularEvents()
        {
            using var scope = ServiceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

            var eventsEnumerable = context.Events.Include(x => x.Promos).AsSplitQuery().AsNoTracking().AsEnumerable();

            var events = eventsEnumerable.Where(x => EventRecords.Any(e => x.Id == e.Record)).ToList();

            // If there are no popular events just get the first lot.
            if (!events.Any())
            {
                return (await context.Events
                        .Include(x => x.Promos)
                        .AsSplitQuery().AsNoTracking().Take(10).Where(x => !x.isCanceled)
                        .OrderBy(x => x.StartLocal).ToListAsync()).Select(x => Mapper.Map<PopularEventViewModel>(x))
                    .ToList();
            }

            return events.Select(x =>
            {
                var mapped = Mapper.Map<PopularEventViewModel>(x);
                mapped.Score = EventRecords.Find(e => x.Id == e.Record).Score;
                return mapped;
            }).OrderBy(x => x.StartLocal).ToList();
        }

        /// <inheritdoc />
        public async Task<List<PopularCategoryViewModel>> GetPopularCategories()
        {
            using var scope = ServiceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

            var categoriesEnumerable = context.Categories.AsSplitQuery().AsNoTracking().AsEnumerable();

            var categories = categoriesEnumerable.Where(x => CategoryRecords.Any(e => x.Id == e.Record))
                .OrderBy(x => CategoryRecords.FirstOrDefault(c => x.Id == c.Record).Score).ToList();

            // If there are no popular categories get all of them.
            if (!categories.Any())
            {
                return (await context.Categories.AsNoTracking().ToListAsync())
                    .Select(x => new PopularCategoryViewModel()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Score = 0
                    }).ToList();
            }

            return categories.Select(x => new PopularCategoryViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                Score = CategoryRecords.Find(c => x.Id == c.Record).Score
            }).ToList();
        }

        /// <inheritdoc />
        public async Task PopulariseEvent(CancellationToken cancellationToken, Guid eventId, DateTime created)
        {
            if (!cancellationToken.IsCancellationRequested)
            {
                Logger.LogInformation("Popularising event {Id}", eventId);

                var popularityRecord = EventRecords.FirstOrDefault(x => x.Record == eventId);

                using var scope = ServiceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

                var e = await context.Events
                    .Include(x => x.EventCategories)
                    .AsNoTracking().AsSplitQuery()
                    .FirstOrDefaultAsync(x => x.Id == eventId, cancellationToken);

                if (e == null)
                {
                    Logger.LogInformation("Event not found");
                    return;
                }

                // if the event is already popular
                if (popularityRecord != null)
                {
                    Logger.LogInformation("Event {Id} is already popular, making more popular", eventId);

                    popularityRecord.Updated = created;
                    popularityRecord.Score++;
                }
                else
                {
                    EventRecords.Add(new PopularityRecord
                    {
                        Added = created,
                        Updated = created,
                        Record = eventId,
                        Score = 1
                    });
                }

                if (e.EventCategories.Any())
                {
                    foreach (var category in e.EventCategories)
                    {
                        await PopulariseCategory(cancellationToken, category.CategoryId, created);
                    }
                }

                // sends popular events to all web-socket clients
                await PopularityHubContext.Clients.All.SendAsync("events", await GetPopularEvents());

                Logger.LogInformation("Popularised event {Id}", eventId);
            }
        }
        
        /// <inheritdoc />
        public async Task PopulariseCategory(CancellationToken cancellationToken, Guid categoryId, DateTime created)
        {
            if (!cancellationToken.IsCancellationRequested)
            {
                Logger.LogInformation("Popularising category {Id}", categoryId);

                var popularityRecord = CategoryRecords.FirstOrDefault(x => x.Record == categoryId);
                
                // if the category is already popular
                if (popularityRecord != null)
                {
                    Logger.LogInformation("Category {Id} is already popular, making more popular", categoryId);

                    popularityRecord.Updated = created;
                    popularityRecord.Score++;
                }
                else
                {
                    using var scope = ServiceProvider.CreateScope();
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

                    var c = await context.Categories.FirstOrDefaultAsync(x => x.Id == categoryId, cancellationToken);

                    if (c == null)
                    {
                        Logger.LogInformation("Category not found");
                        return;
                    }

                    CategoryRecords.Add(new PopularityRecord
                    {
                        Added = created,
                        Updated = created,
                        Record = categoryId,
                        Score = 1
                    });

                }
                
                // sends popular categories to all web-socket clients
                await PopularityHubContext.Clients.All.SendAsync("categories", await GetPopularCategories());

                Logger.LogInformation("Popularised category {Id}", categoryId);
            }
        }

        /// <inheritdoc />
        public bool DownGradeEvent(Guid eventId)
        {
            Logger.LogInformation("Downgrading {Id}'s popularity", eventId);
            return Downgrade(ref EventRecords, eventId);
        }
        
        /// <inheritdoc />
        public bool DownGradeCategory(Guid categoryId)
        {
            Logger.LogInformation("Downgrading {Id}'s popularity", categoryId);
            return Downgrade(ref CategoryRecords, categoryId);
        }

        // Generic method for downgrading event and categories
        private bool Downgrade(ref List<PopularityRecord> popularityRecords, Guid id)
        {
            var eventRecord = popularityRecords.FirstOrDefault(x => x.Record == id);
            if (eventRecord == null)
            {
                Logger.LogInformation("{Id} is not in the popularity list", id);
                return true;
            }

            eventRecord.Score--;

            if (eventRecord.Score < 1)
            {
                Logger.LogInformation("{Id} is no longer popular", id);
                popularityRecords.Remove(eventRecord);
            }

            return false;
        }
        
        /// <inheritdoc />
        public List<PopularityRecord> GetEvents()
        {
            return EventRecords;
        }

        /// <inheritdoc />
        public List<PopularityRecord> GetCategories()
        {
            return CategoryRecords;
        }

        /// <inheritdoc />
        public void CommunicateState()
        {
            Task.Run(async () =>
                await PopularityHubContext.Clients.All.SendAsync("categories", await GetPopularCategories()));
            Task.Run(async () => await PopularityHubContext.Clients.All.SendAsync("events", await GetPopularEvents()));
        }

        private List<PopularityRecord> EventRecords = new();
        private List<PopularityRecord> CategoryRecords = new();
    }
}
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
using OpenEvent.Web.Contexts;
using OpenEvent.Web.Hubs;
using OpenEvent.Web.Models.Category;
using OpenEvent.Web.Models.Event;
using OpenEvent.Web.Models.Popularity;

namespace OpenEvent.Web.Services
{
    // TODO: converting lists to dictionaries would increase performance
    public interface IPopularityService
    {
        Task<List<PopularEventViewModel>> GetPopularEvents();
        Task<List<PopularCategoryViewModel>> GetPopularCategories();
        Task PopulariseEvent(CancellationToken cancellationToken, Guid eventId, DateTime created);
        Task PopulariseCategory(CancellationToken cancellationToken, Guid categoryId, DateTime created);
        bool DownGradeEvent(Guid eventId);
        bool DownGradeCategory(Guid categoryId);
        List<PopularityRecord<Guid>> GetEvents();
        List<PopularityRecord<Guid>> GetCategories();
        void CommunicateState();
    }

    public class PopularityService : IPopularityService
    {
        private readonly ILogger<PopularityService> Logger;
        private readonly IServiceProvider ServiceProvider;
        private readonly IMapper Mapper;
        private readonly IHubContext<PopularityHub> PopularityHubContext;

        public PopularityService(ILogger<PopularityService> logger, IServiceProvider serviceProvider, IMapper mapper,
            IHubContext<PopularityHub> popularityHubContext)
        {
            Logger = logger;
            ServiceProvider = serviceProvider;
            Mapper = mapper;
            PopularityHubContext = popularityHubContext;
        }

        public async Task<List<PopularEventViewModel>> GetPopularEvents()
        {
            using var scope = ServiceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

            var eventsEnumerable = context.Events.AsSplitQuery().AsNoTracking().AsEnumerable();

            var events = eventsEnumerable.Where(x => EventRecords.Any(e => x.Id == e.Record)).ToList();

            // If there are no popular events just get the first lot.
            if (!events.Any())
            {
                return (await context.Events
                        .AsSplitQuery().AsNoTracking().Take(10).Where(x => !x.isCanceled)
                        .OrderBy(x => x.StartLocal).ToListAsync()).Select(x => Mapper.Map<PopularEventViewModel>(x)).ToList();
            }

            return events.Select(x =>
            {
                var mapped = Mapper.Map<PopularEventViewModel>(x);
                mapped.Score = EventRecords.Find(e => x.Id == e.Record).Score;
                return mapped;
            }).OrderBy(x => x.StartLocal).ToList();
        }

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

                if (popularityRecord != null)
                {
                    Logger.LogInformation("Event {Id} is already popular, making more popular", eventId);

                    popularityRecord.Updated = created;
                    popularityRecord.Score++;
                }
                else
                {
                    EventRecords.Add(new PopularityRecord<Guid>
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

                await PopularityHubContext.Clients.All.SendAsync("events", await GetPopularEvents());

                Logger.LogInformation("Popularised event {Id}", eventId);
            }
        }

        public async Task PopulariseCategory(CancellationToken cancellationToken, Guid categoryId, DateTime created)
        {
            if (!cancellationToken.IsCancellationRequested)
            {
                Logger.LogInformation("Popularising category {Id}", categoryId);

                var popularityRecord = CategoryRecords.FirstOrDefault(x => x.Record == categoryId);
                if (popularityRecord != null)
                {
                    Logger.LogInformation("Category {Id} is already popular, making more popular", categoryId);

                    popularityRecord.Updated = created;
                    popularityRecord.Score++;

                    await PopularityHubContext.Clients.All.SendAsync("categories", await GetPopularCategories());
                }

                using var scope = ServiceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

                var c = await context.Categories.FirstOrDefaultAsync(x => x.Id == categoryId, cancellationToken);

                if (c == null)
                {
                    Logger.LogInformation("Category not found");
                    return;
                }

                CategoryRecords.Add(new PopularityRecord<Guid>
                {
                    Added = created,
                    Updated = created,
                    Record = categoryId,
                    Score = 1
                });

                await PopularityHubContext.Clients.All.SendAsync("categories", await GetPopularCategories());

                Logger.LogInformation("Popularised category {Id}", categoryId);
            }
        }

        public bool DownGradeEvent(Guid eventId)
        {
            Logger.LogInformation("Downgrading {Id}'s popularity", eventId);
            return Downgrade(ref EventRecords, eventId);
        }

        public bool DownGradeCategory(Guid categoryId)
        {
            Logger.LogInformation("Downgrading {Id}'s popularity", categoryId);
            return Downgrade(ref CategoryRecords, categoryId);
        }

        private bool Downgrade(ref List<PopularityRecord<Guid>> popularityRecords, Guid id)
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

        public List<PopularityRecord<Guid>> GetEvents()
        {
            return EventRecords;
        }

        public List<PopularityRecord<Guid>> GetCategories()
        {
            return CategoryRecords;
        }

        public void CommunicateState()
        {
            Task.Run(async () =>
                await PopularityHubContext.Clients.All.SendAsync("categories", await GetPopularCategories()));
            Task.Run(async () => await PopularityHubContext.Clients.All.SendAsync("events", await GetPopularEvents()));
        }

        private List<PopularityRecord<Guid>> EventRecords = new();
        private List<PopularityRecord<Guid>> CategoryRecords = new();
    }
}
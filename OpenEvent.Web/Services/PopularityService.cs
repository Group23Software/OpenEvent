using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenEvent.Web.Contexts;
using OpenEvent.Web.Models.Category;
using OpenEvent.Web.Models.Event;
using OpenEvent.Web.Models.Popularity;

namespace OpenEvent.Web.Services
{
    public interface IPopularityService
    {
        Task<List<EventViewModel>> GetPopularEvents();
        Task<List<CategoryViewModel>> GetPopularCategories();
        Task PopulariseEvent(CancellationToken cancellationToken, Guid eventId, DateTime created);
        Task PopulariseCategory(CancellationToken cancellationToken, Guid categoryId, DateTime created);
        bool DownGradeEvent(Guid eventId);
        bool DownGradeCategory(Guid categoryId);
        List<PopularityRecord<Guid>> GetEvents();
        List<PopularityRecord<Guid>> GetCategories();
    }

    public class PopularityService : IPopularityService
    {
        private readonly ILogger<PopularityService> Logger;
        private readonly IServiceProvider ServiceProvider;
        private readonly IMapper Mapper;

        public PopularityService(ILogger<PopularityService> logger, IServiceProvider serviceProvider, IMapper mapper)
        {
            Logger = logger;
            ServiceProvider = serviceProvider;
            Mapper = mapper;
        }

        public async Task<List<EventViewModel>> GetPopularEvents()
        {
            using var scope = ServiceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

            var eventsEnumerable = context.Events.AsSplitQuery().AsNoTracking().AsEnumerable();

            var events = eventsEnumerable.Where(x => EventRecords.Any(e => x.Id == e.Record));

            return events.Select(x => Mapper.Map<EventViewModel>(x)).ToList();
        }

        public async Task<List<CategoryViewModel>> GetPopularCategories()
        {
            using var scope = ServiceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

            var categoriesEnumerable = context.Categories.AsSplitQuery().AsNoTracking().AsEnumerable();

            var categories = categoriesEnumerable.Where(x => CategoryRecords.Any(e => x.Id == e.Record));

            return categories.Select(x => Mapper.Map<CategoryViewModel>(x)).ToList();
        }

        public async Task PopulariseEvent(CancellationToken cancellationToken, Guid eventId, DateTime created)
        {
            if (!cancellationToken.IsCancellationRequested)
            {
                Logger.LogInformation("Popularising event {Id}", eventId);

                var popularityRecord = EventRecords.FirstOrDefault(x => x.Record == eventId);
                if (popularityRecord != null)
                {
                    Logger.LogInformation("Event {Id} is already popular, making more popular", eventId);

                    popularityRecord.Updated = created;
                    popularityRecord.Score++;
                    return;
                }

                using var scope = ServiceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

                var e = await context.Events.FirstOrDefaultAsync(x => x.Id == eventId, cancellationToken);

                if (e == null)
                {
                    Logger.LogInformation("Event not found");
                    return;
                }

                EventRecords.Add(new PopularityRecord<Guid>
                {
                    Added = created,
                    Updated = created,
                    Record = eventId,
                    Score = 1
                });

                Logger.LogInformation("Popularised event {Id}", eventId);
            }
        }

        public async Task PopulariseCategory(CancellationToken cancellationToken, Guid categoryId, DateTime created)
        {
            if (!cancellationToken.IsCancellationRequested)
            {
                Logger.LogInformation("Popularising category {Id}", categoryId);

                var popularityRecord = EventRecords.FirstOrDefault(x => x.Record == categoryId);
                if (popularityRecord != null)
                {
                    Logger.LogInformation("Category {Id} is already popular, making more popular", categoryId);

                    popularityRecord.Updated = created;
                    popularityRecord.Score++;
                    return;
                }

                using var scope = ServiceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

                var e = await context.Events.FirstOrDefaultAsync(x => x.Id == categoryId, cancellationToken);

                if (e == null)
                {
                    Logger.LogInformation("Event not found");
                    return;
                }

                CategoryRecords.Add(new PopularityRecord<Guid>
                {
                    Added = created,
                    Updated = created,
                    Record = categoryId,
                    Score = 1
                });

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

        private List<PopularityRecord<Guid>> EventRecords = new();
        private List<PopularityRecord<Guid>> CategoryRecords = new();
    }
}
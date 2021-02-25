using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenEvent.Web.Contexts;
using OpenEvent.Web.Models.Analytic;

namespace OpenEvent.Web.Services
{
    public interface IAnalyticsService
    {
        void CaptureSearch(string keyword, string searchParams, Guid userId);
        void CapturePageView(Guid eventId, Guid? userId);
    }

    /// <summary>
    /// Service providing all analytics logic.
    /// </summary>
    public class AnalyticsService : IAnalyticsService
    {
        private readonly ILogger<AnalyticsService> Logger;
        private readonly IServiceScopeFactory ScopeFactory;

        private Queue<AnalyticEvent> AnalyticEvents;

        public AnalyticsService(ILogger<AnalyticsService> logger, IServiceScopeFactory serviceScopeFactory)
        {
            Logger = logger;
            ScopeFactory = serviceScopeFactory;
        }

        /// <summary>
        /// Captures a search event and saves to database.
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="searchParams"></param>
        /// <param name="userId"></param>
        public void CaptureSearch(string keyword, string searchParams, Guid userId)
        {
            Logger.LogInformation("Capturing search analytic");
            Task.Run(async () =>
            {
                using var scope = ScopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

                var u = await context.Users.FirstOrDefaultAsync(x => x.Id == userId);

                if (u == null)
                {
                    Logger.LogInformation("User was not found when capturing event");
                    return;
                }

                SearchEvent searchEvent = new()
                {
                    Created = DateTime.Now,
                    Search = keyword,
                    Params = searchParams,
                    User = u
                };

                await Save(context, searchEvent);
                Logger.LogInformation("Analytic has been processed");
            });
        }

        /// <summary>
        /// Captures page view event and saves to database.
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="userId"></param>
        public void CapturePageView(Guid eventId, Guid? userId)
        {
            Logger.LogInformation("Capturing page view analytic");
            Task.Run(async () =>
            {
                using var scope = ScopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

                var u = await context.Users.FirstOrDefaultAsync(x => x.Id == userId);
                var e = await context.Events.FirstOrDefaultAsync(x => x.Id == eventId);

                if (e == null)
                {
                    Logger.LogInformation("Event was not found when capturing event");
                    return;
                }

                PageViewEvent pageViewEvent = new()
                {
                    Created = DateTime.Now,
                    Event = e,
                    User = u
                };

                await Save(context, pageViewEvent);
                Logger.LogInformation("Analytic has been processed");
            });
        }

        private async Task Save(ApplicationContext context, AnalyticEvent analyticEvent)
        {
            await context.AddAsync(analyticEvent);
            try
            {
                await context.SaveChangesAsync();
            }
            catch
            {
                Logger.LogInformation("Failed to save analytics event");
            }
            finally
            {
                context = default;
            }
        }
    }
}
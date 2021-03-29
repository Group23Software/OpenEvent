using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenEvent.Data.Models.Analytic;
using OpenEvent.Web.Contexts;

namespace OpenEvent.Web.Services
{
    /// <inheritdoc />
    public class AnalyticsService : IAnalyticsService
    {
        private readonly ILogger<AnalyticsService> Logger;
        private readonly IServiceProvider ServiceProvider;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="serviceProvider"></param>
        public AnalyticsService(ILogger<AnalyticsService> logger, IServiceProvider serviceProvider)
        {
            Logger = logger;
            ServiceProvider = serviceProvider;
        }
        
        /// <inheritdoc />
        public async Task CaptureSearchAsync(CancellationToken cancellationToken, string keyword, string searchParams,
            Guid? userId, DateTime created)
        {
            if (!cancellationToken.IsCancellationRequested)
            {
                Logger.LogInformation("Capturing search analytic");

                using var scope = ServiceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

                var u = await context.Users.FirstOrDefaultAsync(x => x.Id == userId);

                if (u == null)
                {
                    Logger.LogInformation("User was not found when capturing event");
                    return;
                }

                SearchEvent searchEvent = new()
                {
                    Created = created,
                    Search = keyword,
                    Params = searchParams,
                    User = u
                };

                await Save(context, searchEvent);
                Logger.LogInformation("Analytic has been processed");
            }
        }

        /// <inheritdoc />
        public async Task CapturePageViewAsync(CancellationToken cancellationToken, Guid eventId, Guid? userId,
            DateTime created)
        {
            if (!cancellationToken.IsCancellationRequested)
            {
                Logger.LogInformation("Capturing page view analytic");

                using var scope = ServiceProvider.CreateScope();
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
                    Created = created,
                    Event = e,
                    User = u
                };

                await Save(context, pageViewEvent);
                Logger.LogInformation("Analytic has been processed");
            }
        }

        /// <inheritdoc />
        public async Task CaptureTicketVerifyAsync(CancellationToken cancellationToken, Guid ticketId, Guid eventId,
            DateTime created)
        {
            if (!cancellationToken.IsCancellationRequested)
            {
                Logger.LogInformation("Capturing ticket verification analytic");

                using var scope = ServiceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

                var ticket = await context.Tickets.Include(x => x.User).FirstOrDefaultAsync(x => x.Id == ticketId);
                var e = await context.Events.FirstOrDefaultAsync(x => x.Id == eventId);

                if (ticket == null)
                {
                    Logger.LogInformation("Ticket was not found when capturing event");
                    return;
                }

                if (e == null)
                {
                    Logger.LogInformation("Event was not found when capturing event");
                    return;
                }

                TicketVerificationEvent ticketVerificationEvent = new TicketVerificationEvent
                {
                    Created = created,
                    Event = e,
                    Ticket = ticket,
                    User = ticket.User
                };

                await Save(context, ticketVerificationEvent);
                Logger.LogInformation("Analytic has been processed");
            }
        }

        // Saves all tracked entities
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
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
        void CaptureTicketVerify(Guid ticketId, Guid eventId);
    }

    /// <summary>
    /// Service providing all analytics logic.
    /// </summary>
    public class AnalyticsService : IAnalyticsService
    {
        private readonly ILogger<AnalyticsService> Logger;
        private readonly IServiceScopeFactory ScopeFactory;
        private readonly IRecommendationService RecommendationService;
        
        private Queue<AnalyticEvent> AnalyticEvents;

        public AnalyticsService(ILogger<AnalyticsService> logger, IServiceScopeFactory serviceScopeFactory,IRecommendationService recommendationService)
        {
            Logger = logger;
            ScopeFactory = serviceScopeFactory;
            RecommendationService = recommendationService;
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

        public void CaptureTicketVerify(Guid ticketId, Guid eventId)
        {
            Logger.LogInformation("Capturing ticket verify analytic");
            Task.Run(async () =>
            {
                using var scope = ScopeFactory.CreateScope();
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
                    Created = DateTime.Now,
                    Event = e,
                    Ticket = ticket,
                    User = ticket.User
                };

                await Save(context, ticketVerificationEvent);
                Logger.LogInformation("Analytic has been processed");
                
                RecommendationService.Influence(ticket.User.Id,e.Id,Influence.Verify);
            });
        }

        // public void CapturePurchase(string transactionId, Guid userId)
        // {
        //     Logger.LogInformation("Capturing purchase analytic");
        //     Task.Run(async () =>
        //     {
        //         using var scope = ScopeFactory.CreateScope();
        //         var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
        //
        //         var transaction =
        //             await context.Transactions.FirstOrDefaultAsync(x => x.StripeIntentId == transactionId);
        //         
        //         var user = await context.Users.FirstOrDefaultAsync(x => x.Id == userId);
        //
        //         if (transaction == null)
        //         {
        //             Logger.LogInformation("Transaction was not found when capturing event");
        //             return;
        //         }
        //         
        //         if (user == null)
        //         {
        //             Logger.LogInformation("User was not found when capturing event");
        //             return;
        //         }
        //
        //         TicketPurchaseEvent ticketPurchaseEvent = new TicketPurchaseEvent
        //         {
        //             Created = DateTime.Now,
        //             User = user,
        //             Event = 
        //         };
        //
        //     });
        // }

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
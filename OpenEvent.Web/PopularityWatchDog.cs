using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenEvent.Web.Services;

namespace OpenEvent.Web
{
    /// <summary>
    /// Watchdog service that checks if popular events and categories should be demoted 
    /// </summary>
    public class PopularityWatchDog : IHostedService, IDisposable
    {
        private readonly ILogger<PopularityWatchDog> Logger;
        private readonly IPopularityService PopularityService;

        // Check times
        private readonly TimeSpan CheckSpan = TimeSpan.FromMinutes(10);
        private readonly TimeSpan DowngradeSpan = TimeSpan.FromHours(1);
        
        private Timer Timer;

        /// <summary>
        /// Constructor, injects logger and popularity service
        /// </summary>
        /// <param name="logger">ILogger</param>
        /// <param name="popularityService">IPopularityService</param>
        public PopularityWatchDog(ILogger<PopularityWatchDog> logger, IPopularityService popularityService)
        {
            Logger = logger;
            PopularityService = popularityService;
        }

        /// <summary>
        /// Start method for building and initialising the service
        /// </summary>
        /// <param name="cancellationToken">Cancel</param>
        /// <returns>
        /// Completed task once built
        /// </returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation("Popularity watchdog running");

            Timer = new Timer(Process, null, TimeSpan.Zero, CheckSpan);

            return Task.CompletedTask;
        }

        private void Process(object state)
        {
            Logger.LogInformation("Checking popularity");

            for (var i = 0; i < PopularityService.GetEvents().Count; i++)
            {
                // If the popularity of the event hasn't increased in x time then downgrade it's popularity.
                var timeFrom = DateTime.Now - PopularityService.GetEvents()[i].Updated;
                
                if (timeFrom >= DowngradeSpan)
                {
                    if (PopularityService.DownGradeEvent(PopularityService.GetEvents()[i].Record)) i--;
                }
            }
            
            for (var i = 0; i < PopularityService.GetCategories().Count; i++)
            {
                // If the popularity of the event hasn't increased in x time then downgrade it's popularity.
                var timeFrom = DateTime.Now - PopularityService.GetCategories()[i].Updated;
                
                if (timeFrom >= DowngradeSpan)
                {
                    if (PopularityService.DownGradeCategory(PopularityService.GetCategories()[i].Record)) i--;
                }
            }
            
            // Sending updated event and category arrays to client using web-socket.
            PopularityService.CommunicateState();

            Logger.LogInformation("Finished checking popularity");
        }

        /// <summary>
        /// Runs when disposed or cancellation token
        /// </summary>
        /// <param name="cancellationToken">Cancel</param>
        /// <returns>
        /// Completed task once stopped
        /// </returns>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation("Stopping popularity watchdog");

            Timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        /// <summary>
        /// Disposes timer once finished
        /// </summary>
        public void Dispose()
        {
            Timer?.Dispose();
        }
    }
}
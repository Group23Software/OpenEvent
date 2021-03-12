using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenEvent.Web.Services;

namespace OpenEvent.Web
{
    public class PopularityWatchDog : IHostedService, IDisposable
    {
        private readonly ILogger<PopularityWatchDog> Logger;
        private readonly IPopularityService PopularityService;

        private readonly TimeSpan CheckSpan = TimeSpan.FromMinutes(10);
        private readonly TimeSpan DowngradeSpan = TimeSpan.FromHours(1);

        // Using timer will not wait for the last operation to finish.
        private Timer Timer;

        public PopularityWatchDog(ILogger<PopularityWatchDog> logger, IPopularityService popularityService)
        {
            Logger = logger;
            PopularityService = popularityService;
        }

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
                Logger.LogInformation("time from {Time}", timeFrom);
                if (timeFrom >= DowngradeSpan)
                {
                    if (PopularityService.DownGradeEvent(PopularityService.GetEvents()[i].Record)) i--;
                }
            }
            
            for (var i = 0; i < PopularityService.GetCategories().Count; i++)
            {
                // If the popularity of the event hasn't increased in x time then downgrade it's popularity.
                var timeFrom = DateTime.Now - PopularityService.GetCategories()[i].Updated;
                Logger.LogInformation("time from {Time}", timeFrom);
                if (timeFrom >= DowngradeSpan)
                {
                    if (PopularityService.DownGradeCategory(PopularityService.GetCategories()[i].Record)) i--;
                }
            }
            
            // Sending updated event and category arrays to client.
            PopularityService.CommunicateState();

            Logger.LogInformation("Finished checking popularity");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation("Stopping popularity watchdog");

            Timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            Timer?.Dispose();
        }
    }
}
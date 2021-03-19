using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace OpenEvent.Web
{
    public class BackGroundWorkService : BackgroundService
    {
        private readonly ILogger<BackGroundWorkService> Logger;
        private IWorkQueue WorkQueue { get; }

        public BackGroundWorkService(ILogger<BackGroundWorkService> logger,IWorkQueue workQueue)
        {
            Logger = logger;
            WorkQueue = workQueue;
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Logger.LogInformation("Running Background Work Service");
            await Process(stoppingToken);
        }

        private async Task Process(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var nextWork = await WorkQueue.DequeueAsync(cancellationToken);

                try
                {
                    Logger.LogInformation("Working on next");
                    await nextWork(cancellationToken);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "Error occurred executing {WorkItem}", nameof(nextWork));
                }
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken);
        }
    }
}
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace OpenEvent.Web
{
    /// <summary>
    /// Background service that works on all work in queue
    /// </summary>
    public class BackGroundWorkService : BackgroundService
    {
        private readonly ILogger<BackGroundWorkService> Logger;
        private IWorkQueue WorkQueue { get; }

        /// <inheritdoc />
        public BackGroundWorkService(ILogger<BackGroundWorkService> logger,IWorkQueue workQueue)
        {
            Logger = logger;
            WorkQueue = workQueue;
        }
        
        /// <summary>
        /// Method run when service is created
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Logger.LogInformation("Running Background Work Service");
            await Process(stoppingToken);
        }

        private async Task Process(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                // Waits for the next work item from the queue
                var nextWork = await WorkQueue.DequeueAsync(cancellationToken);

                try
                {
                    Logger.LogInformation("Working on next");
                    // Runs the task
                    await nextWork(cancellationToken);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "Error occurred executing {WorkItem}", nameof(nextWork));
                }
            }
        }

        /// <summary>
        /// Runs when the service is stopped
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken);
        }
    }
}
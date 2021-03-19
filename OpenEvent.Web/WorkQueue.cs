using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace OpenEvent.Web
{
    public interface IWorkQueue
    {
        void QueueWork(Func<CancellationToken,Task> workItem);

        Task<Func<CancellationToken,Task>> DequeueAsync(
            CancellationToken cancellationToken);
    }

    public class WorkQueue : IWorkQueue
    {
        private readonly ILogger<WorkQueue> Logger;

        private readonly ConcurrentQueue<Func<CancellationToken, Task>> Queue = new();

        // private readonly SemaphoreSlim Signal = new(0,10);
        private readonly SemaphoreSlim Signal = new(0);
        
        public WorkQueue(ILogger<WorkQueue> logger)
        {
            Logger = logger;
        }

        public void QueueWork(Func<CancellationToken,Task> work)
        {
            if (work == null) throw new ArgumentNullException(nameof(work));

            Logger.LogInformation("Queueing 1 of {QueueCount}", Queue.Count);

            Queue.Enqueue(work);    
            Signal.Release();
        }

        public async Task<Func<CancellationToken,Task>> DequeueAsync(CancellationToken cancellationToken)
        {
            await Signal.WaitAsync(cancellationToken);
            Queue.TryDequeue(out var work);
            
            return work;
        }
    }
}
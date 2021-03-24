using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace OpenEvent.Web
{
    /// <summary>
    /// Defines a task queue
    /// </summary>
    public interface IWorkQueue
    {
        /// <summary>
        /// Queues a task into the work queue
        /// </summary>
        /// <param name="work">A Task that will be queued with cancellation token</param>
        void QueueWork(Func<CancellationToken, Task> work);

        /// <summary>
        /// Takes the next task from the queue
        /// </summary>
        /// <param name="cancellationToken">Task cancel token</param>
        /// <returns>
        /// Function of token, task
        /// </returns>
        Task<Func<CancellationToken, Task>> DequeueAsync(
            CancellationToken cancellationToken);
    }

    /// <inheritdoc />
    public class WorkQueue : IWorkQueue
    {
        private readonly ILogger<WorkQueue> Logger;
        private readonly ConcurrentQueue<Func<CancellationToken, Task>> Queue = new();
        private readonly SemaphoreSlim Signal = new(0);

        /// <summary>
        /// Constructor, injects logger
        /// </summary>
        /// <param name="logger">ILogger</param>
        public WorkQueue(ILogger<WorkQueue> logger)
        {
            Logger = logger;
        }

        /// <summary>
        /// Queues a task into the work queue
        /// </summary>
        /// <param name="work">A Task that will be queued with cancellation token</param>
        /// <exception cref="ArgumentNullException">Thrown if work does not exist</exception>
        public void QueueWork(Func<CancellationToken, Task> work)
        {
            if (work == null) throw new ArgumentNullException(nameof(work));

            Logger.LogInformation("Queueing {Name} which is 1 of {QueueCount}", nameof(work), Queue.Count);

            Queue.Enqueue(work);

            // Releases the semaphore to allow threads to enter
            Signal.Release();
        }

        /// <summary>
        /// Takes the next task from the queue
        /// </summary>
        /// <param name="cancellationToken">Task cancel token</param>
        /// <returns>
        /// Function of token, task
        /// </returns>
        public async Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken)
        {
            await Signal.WaitAsync(cancellationToken);
            Queue.TryDequeue(out var work);

            return work;
        }
    }
}
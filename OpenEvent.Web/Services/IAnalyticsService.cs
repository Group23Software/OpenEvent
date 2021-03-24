using System;
using System.Threading;
using System.Threading.Tasks;

namespace OpenEvent.Web.Services
{
    /// <summary>
    /// Service providing all analytics logic.
    /// </summary>
    public interface IAnalyticsService
    {
        /// <summary>
        /// Captures a search analytic event
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="keyword"></param>
        /// <param name="searchParams"></param>
        /// <param name="userId"></param>
        /// <param name="created"></param>
        /// <returns>Completed task once the analytic has been saved</returns>
        Task CaptureSearchAsync(CancellationToken cancellationToken, string keyword, string searchParams, Guid? userId,
            DateTime created);

        /// <summary>
        /// Captures a page view analytic event
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="eventId"></param>
        /// <param name="userId"></param>
        /// <param name="created"></param>
        /// <returns>Completed task once the analytic has been saved</returns>
        Task CapturePageViewAsync(CancellationToken cancellationToken, Guid eventId, Guid? userId, DateTime created);

        /// <summary>
        /// Captures a ticket verification analytic event
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="ticketId"></param>
        /// <param name="eventId"></param>
        /// <param name="created"></param>
        /// <returns>Completed task once the analytic has been saved</returns>
        Task CaptureTicketVerifyAsync(CancellationToken cancellationToken, Guid ticketId, Guid eventId,
            DateTime created);
    }
}
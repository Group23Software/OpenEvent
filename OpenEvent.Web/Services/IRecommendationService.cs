using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OpenEvent.Data.Models.Event;
using OpenEvent.Data.Models.Recommendation;

namespace OpenEvent.Web.Services
{
    /// <summary>
    /// Service for influencing the user's recommendation score based on events
    /// </summary>
    public interface IRecommendationService
    {
        /// <summary>
        /// Influences the user's recommendation score 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="userId"></param>
        /// <param name="eventId"></param>
        /// <param name="influence"></param>
        /// <param name="created"></param>
        /// <returns>Completed task when finished</returns>
        Task InfluenceAsync(CancellationToken cancellationToken, Guid userId, Guid eventId, Influence influence,
            DateTime created);

        /// <summary>
        /// Influences the user's recommendation score after a search is made
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="userId"></param>
        /// <param name="keyword"></param>
        /// <param name="searchFilters"></param>
        /// <param name="created"></param>
        /// <returns>Completed task when finished</returns>
        Task InfluenceAsync(CancellationToken cancellationToken, Guid userId, string keyword,
            List<SearchFilter> searchFilters, DateTime created);
    }
}
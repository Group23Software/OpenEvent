using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OpenEvent.Data.Models.Category;
using OpenEvent.Data.Models.Event;
using OpenEvent.Data.Models.Popularity;

namespace OpenEvent.Web.Services
{
    /// <summary>
    /// Service maintaining popular events and categories in real time
    /// </summary>
    public interface IPopularityService
    {
        /// <summary>
        /// Gets all currently popular events
        /// </summary>
        /// <returns>
        /// List of events
        /// </returns>
        Task<List<PopularEventViewModel>> GetPopularEvents();

        /// <summary>
        /// Get all currently popular categories
        /// </summary>
        /// <returns></returns>
        Task<List<PopularCategoryViewModel>> GetPopularCategories();

        /// <summary>
        /// Increase an events popularity
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="eventId"></param>
        /// <param name="created"></param>
        /// <returns>Completed task once finished popularising</returns>
        Task PopulariseEvent(CancellationToken cancellationToken, Guid eventId, DateTime created);

        /// <summary>
        /// Increase a categories popularity
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="categoryId"></param>
        /// <param name="created"></param>
        /// <returns>Completed task once finished popularising</returns>
        Task PopulariseCategory(CancellationToken cancellationToken, Guid categoryId, DateTime created);

        /// <summary>
        /// Downgrades the event by 1
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns>true if the event was not popular</returns>
        bool DownGradeEvent(Guid eventId);

        /// <summary>
        /// Downgrades the category by 1
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns>true if the category was not popular</returns>
        bool DownGradeCategory(Guid categoryId);

        /// <summary>
        /// Gets the popular event records
        /// </summary>
        /// <returns>Returns a list of popularity records</returns>
        List<PopularityRecord> GetEvents();

        /// <summary>
        /// Gets the popular category records
        /// </summary>
        /// <returns></returns>
        List<PopularityRecord> GetCategories();

        /// <summary>
        /// Sends popular events and categories to all web-socket clients
        /// </summary>
        void CommunicateState();
    }
}
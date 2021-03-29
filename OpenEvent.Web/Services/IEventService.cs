using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OpenEvent.Data.Models.Analytic;
using OpenEvent.Data.Models.Category;
using OpenEvent.Data.Models.Event;

namespace OpenEvent.Web.Services
{
    /// <summary>
    /// Service providing all event logic.
    /// </summary>
    public interface IEventService
    {
        // Host methods
        /// <summary>
        /// Creates a new event.
        /// Gets event location from azure maps API.
        /// </summary>
        /// <param name="createEventBody"><see cref="CreateEventBody"/></param>
        /// <returns>Returns the newly created event</returns>
        Task<EventViewModel> Create(CreateEventBody createEventBody);

        /// <summary>
        /// Sets IsCanceled true.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Completed task when one the event is canceled</returns>
        Task Cancel(Guid id);

        /// <summary>
        /// Gets all of the users hosted events.
        /// </summary>
        /// <param name="hostId"></param>
        /// <returns>Returns list of events</returns>
        Task<List<EventHostModel>> GetAllHosts(Guid hostId);


        //public methods
        /// <summary>
        /// Gets all public facing data for an event.
        /// </summary>
        /// <param name="id">Event id</param>
        /// <param name="userId">User id (for analytics)</param>
        /// <returns>Returns event</returns>
        Task<EventDetailModel> GetForPublic(Guid id, Guid? userId);

        /// <summary>
        /// Searches events.
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="filters"></param>
        /// <param name="userId"></param>
        /// <returns>List of events</returns>
        Task<List<EventViewModel>> Search(string keyword, List<SearchFilter> filters, Guid? userId);

        /// <summary>
        /// Gets all event categories.
        /// </summary>
        /// <returns>List of categories</returns>
        Task<List<Category>> GetAllCategories();

        /// <summary>
        /// Gets all data for event.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Event</returns>
        Task<EventHostModel> GetForHost(Guid id);

        /// <summary>
        /// Updates the event.
        /// </summary>
        /// <param name="updateEventBody"></param>
        /// <returns>Completed task one the event has been updated</returns>
        Task Update(UpdateEventBody updateEventBody);

        /// <summary>
        /// Gets recommended events based on a users recommendation scores
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>List of events</returns>
        Task<List<EventViewModel>> GetRecommended(Guid id);

        /// <summary>
        /// Gets events analytics
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns><see cref="EventAnalytics"/></returns>
        Task<EventAnalytics> GetAnalytics(Guid id);
    }
}
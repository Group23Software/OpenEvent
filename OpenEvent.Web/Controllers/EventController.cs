using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenEvent.Web.Contexts;
using OpenEvent.Web.Models.Analytic;
using OpenEvent.Web.Models.Category;
using OpenEvent.Web.Models.Event;
using OpenEvent.Web.Models.Recommendation;
using OpenEvent.Web.Services;
using OpenEvent.Web.UserOwnsEvent;

namespace OpenEvent.Web.Controllers
{
    /// <summary>
    /// API controller for all event related endpoints.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class EventController : ControllerBase
    {
        private readonly IEventService EventService;
        private readonly ILogger<EventController> Logger;
        private readonly IRecommendationService RecommendationService;
        private readonly IWorkQueue WorkQueue;

        /// <inheritdoc />
        public EventController(IEventService eventService, ILogger<EventController> logger,
            IRecommendationService recommendationService, IWorkQueue workQueue)
        {
            EventService = eventService;
            Logger = logger;
            RecommendationService = recommendationService;
            WorkQueue = workQueue;
        }

        /// <summary>
        /// Endpoint for creating an event.
        /// </summary>
        /// <param name="createEventBody"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<EventViewModel>> Create([FromBody] CreateEventBody createEventBody)
        {
            try
            {
                var result = await EventService.Create(createEventBody);
                return result;
            }
            catch (Exception e)
            {
                Logger.LogInformation(e.ToString());
                return BadRequest(e);
            }
        }

        /// <summary>
        /// Endpoint to cancel an event with user owns filter.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [UserOwnsEvent]
        [HttpPost("cancel")]
        public async Task<ActionResult> Cancel(Guid id)
        {
            try
            {
                await EventService.Cancel(id);
                return Ok();
            }
            catch (Exception e)
            {
                Logger.LogInformation(e.ToString());
                return BadRequest(e);
            }
        }

        /// <summary>
        /// Endpoint to get all data needed to display an event to the public.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("public")]
        public async Task<ActionResult<EventDetailModel>> GetForPublic(Guid id, Guid? userId)
        {
            try
            {
                var result = await EventService.GetForPublic(id, userId);
                return result;
            }
            catch (Exception e)
            {
                Logger.LogInformation(e.ToString());
                return BadRequest(e);
            }
        }

        /// <summary>
        /// Endpoint to get all the events a user is hosting.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("host")]
        public async Task<ActionResult<List<EventHostModel>>> GetAllHosts(Guid id)
        {
            var result = await EventService.GetAllHosts(id);
            return result;
        }

        /// <summary>
        /// Endpoint to get all the data needed when configuring an event with user owns filter.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // [UserOwnsEvent]
        [HttpGet("forHost")]
        public async Task<ActionResult<EventHostModel>> GetForHost(Guid id)
        {
            try
            {
                var result = await EventService.GetForHost(id);
                return result;
            }
            catch (Exception e)
            {
                Logger.LogInformation(EventService.ToString());
                return BadRequest(e);
            }
        }

        /// <summary>
        /// Endpoint for updating an event with user owns filter.
        /// </summary>
        /// <param name="updateEventBody"></param>
        /// <returns></returns>
        [UserOwnsEvent]
        [HttpPost("update")]
        public async Task<ActionResult> Update(UpdateEventBody updateEventBody)
        {
            try
            {
                await EventService.Update(updateEventBody);
                return Ok();
            }
            catch (Exception e)
            {
                Logger.LogInformation(e.ToString());
                return BadRequest(e);
            }
        }

        /// <summary>
        /// Endpoint to get all the available event categories.
        /// </summary>
        /// <returns></returns>
        [HttpGet("categories")]
        public async Task<ActionResult<List<Category>>> GetAllCategories()
        {
            return await EventService.GetAllCategories();
        }

        /// <summary>
        /// Endpoint for searching events with keywords and defined filters.
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="filters"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost("search")]
        public async Task<ActionResult<List<EventViewModel>>> Search(string keyword, List<SearchFilter> filters,
            Guid userId)
        {
            try
            {
                var results = await EventService.Search(keyword, filters, userId);
                return results;
            }
            catch (Exception e)
            {
                Logger.LogInformation(e.ToString());
                return BadRequest(e);
            }
        }

        /// <summary>
        /// Endpoint for getting a list of recommended events based on the user
        /// </summary>
        /// <param name="id">User's id</param>
        /// <returns>List of events</returns>
        [HttpGet("explore")]
        public async Task<ActionResult<List<EventViewModel>>> Explore(Guid id)
        {
            try
            {
                return await EventService.GetRecommended(id);
            }
            catch (Exception e)
            {
                Logger.LogInformation(e.ToString());
                return BadRequest(e);
            }
        }

        /// <summary>
        /// Endpoint for getting all the events analytics
        /// </summary>
        /// <param name="id">Event id</param>
        /// <returns>Event analytics <see cref="EventAnalytics"/></returns>
        [HttpGet("analytics")]
        public async Task<ActionResult<EventAnalytics>> GetAnalytics(Guid id)
        {
            try
            {
                return await EventService.GetAnalytics(id);
            }
            catch (Exception e)
            {
                Logger.LogInformation(e.ToString());
                return BadRequest(e);
            }
        }

        /// <summary>
        /// Endpoint for down voting user's recommendation scores based on the event
        /// </summary>
        /// <param name="userId">User's id</param>
        /// <param name="eventId">Event's id</param>
        /// <returns>Ok</returns>
        [HttpPost("downvote")]
        public ActionResult DownVote(Guid userId, Guid eventId)
        {
            WorkQueue.QueueWork(token =>
                RecommendationService.InfluenceAsync(token, userId, eventId, Influence.DownVote, DateTime.Now));
            return Ok();
        }
    }
}
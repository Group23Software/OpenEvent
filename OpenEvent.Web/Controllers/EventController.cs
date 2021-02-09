using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenEvent.Web.Contexts;
using OpenEvent.Web.Models.Category;
using OpenEvent.Web.Models.Event;
using OpenEvent.Web.Services;
using OpenEvent.Web.UserOwnsEvent;

namespace OpenEvent.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventController : ControllerBase
    {
        private readonly IEventService EventService;
        private readonly ILogger<EventController> Logger;

        public EventController(IEventService eventService, ILogger<EventController> logger)
        {
            EventService = eventService;
            Logger = logger;
        }

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

        [HttpGet("public")]
        public async Task<ActionResult<EventDetailModel>> GetForPublic(Guid id)
        {
            try
            {
                var result = await EventService.GetForPublic(id);
                return result;
            }
            catch (Exception e)
            {
                Logger.LogInformation(e.ToString());
                return BadRequest(e);
            }
        }

        [HttpGet("host")]
        public async Task<ActionResult<List<EventHostModel>>> GetAllHosts(Guid id)
        {
            var result = await EventService.GetAllHosts(id);
            return result;
        }

        [UserOwnsEvent]
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
                Logger.LogInformation(EventService.ToString());
                return BadRequest(e);
            }
        }

        [HttpGet("categories")]
        public async Task<ActionResult<List<Category>>> GetAllCategories()
        {
            return await EventService.GetAllCategories();
        }
    }
}
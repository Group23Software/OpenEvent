using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenEvent.Web.Models.Category;
using OpenEvent.Web.Models.Event;
using OpenEvent.Web.Services;

namespace OpenEvent.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PopularityController : ControllerBase
    {
        private readonly ILogger<PopularityController> Logger;
        private readonly IPopularityService PopularityService;

        public PopularityController(ILogger<PopularityController> logger, IPopularityService popularityService)
        {
            Logger = logger;
            PopularityService = popularityService;
        }

        [AllowAnonymous]
        [HttpGet("events")]
        public async Task<ActionResult<List<EventViewModel>>> GetPopularEvents()
        {
            try
            {
                return await PopularityService.GetPopularEvents();
            }
            catch (Exception e)
            {
                Logger.LogError(e.ToString());
                return BadRequest(e);
            }
        }
        
        [AllowAnonymous]
        [HttpGet("categories")]
        public async Task<ActionResult<List<CategoryViewModel>>> GetPopularCategories()
        {
            try
            {
                return await PopularityService.GetPopularCategories();
            }
            catch (Exception e)
            {
                Logger.LogError(e.ToString());
                return BadRequest(e);
            }
        }
    }
}
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
    /// <inheritdoc />
    [ApiController]
    [Route("api/[controller]")]
    public class PopularityController : ControllerBase
    {
        private readonly ILogger<PopularityController> Logger;
        private readonly IPopularityService PopularityService;

        /// <inheritdoc />
        public PopularityController(ILogger<PopularityController> logger, IPopularityService popularityService)
        {
            Logger = logger;
            PopularityService = popularityService;
        }

        /// <summary>
        /// Endpoint for getting all popular events
        /// </summary>
        /// <returns>List of events</returns>
        [AllowAnonymous]
        [HttpGet("events")]
        public async Task<ActionResult<List<PopularEventViewModel>>> GetPopularEvents()
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
        
        /// <summary>
        /// Endpoint for getting all popular categories
        /// </summary>
        /// <returns>List of categories</returns>
        [AllowAnonymous]
        [HttpGet("categories")]
        public async Task<ActionResult<List<PopularCategoryViewModel>>> GetPopularCategories()
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
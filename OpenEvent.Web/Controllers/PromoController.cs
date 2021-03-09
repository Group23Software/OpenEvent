using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenEvent.Web.Models.Promo;
using OpenEvent.Web.Services;

namespace OpenEvent.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PromoController : ControllerBase
    {
        private readonly ILogger<PromoController> Logger;
        private readonly IPromoService PromoService;

        public PromoController(ILogger<PromoController> logger, IPromoService promoService)
        {
            Logger = logger;
            PromoService = promoService;
        }

        [HttpPost]
        public async Task<ActionResult<PromoViewModel>> Create(CreatePromoBody createPromoBody)
        {
            try
            {
                var result = await PromoService.Create(createPromoBody);
                return result;
            }
            catch (Exception e)
            {
                Logger.LogInformation("Create exception {Exception}", e.ToString());
                return BadRequest(e);
            }
        }

        [HttpPost("update")]
        public async Task<ActionResult<PromoViewModel>> Update(UpdatePromoBody updatePromoBody)
        {
            try
            {
                var result = await PromoService.Update(updatePromoBody);
                return result;
            }
            catch (Exception e)
            {
                Logger.LogInformation("Update exception {Exception}", e.ToString());
                return BadRequest(e);
            }
        }

        [HttpDelete]
        public async Task<ActionResult> Destroy(Guid id)
        {
            try
            {
                await PromoService.Destroy(id);
                return Ok();
            }
            catch (Exception e)
            {
                Logger.LogInformation("Destroy exception {Exception}", e.ToString());
                return BadRequest(e);
            }
        }
        
    }
}
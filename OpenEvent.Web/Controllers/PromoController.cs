using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenEvent.Data.Models.Promo;
using OpenEvent.Web.Services;

namespace OpenEvent.Web.Controllers
{
    /// <inheritdoc />
    [ApiController]
    [Route("api/[controller]")]
    public class PromoController : ControllerBase
    {
        private readonly ILogger<PromoController> Logger;
        private readonly IPromoService PromoService;

        /// <inheritdoc />
        public PromoController(ILogger<PromoController> logger, IPromoService promoService)
        {
            Logger = logger;
            PromoService = promoService;
        }

        /// <summary>
        /// Endpoint for creating a new promo
        /// </summary>
        /// <param name="createPromoBody"></param>
        /// <returns>Newly created promo</returns>
        [HttpPost]
        public async Task<ActionResult<PromoViewModel>> Create(CreatePromoBody createPromoBody)
        {
            try
            {
                Logger.LogInformation("Creating {Discount}% promo for {Id}", createPromoBody.Discount, createPromoBody.EventId);
                var result = await PromoService.Create(createPromoBody);
                return result;
            }
            catch (Exception e)
            {
                Logger.LogInformation("Create exception {Exception}", e.ToString());
                return BadRequest(e);
            }
        }

        /// <summary>
        /// Endpoint for updating a promo
        /// </summary>
        /// <param name="updatePromoBody"></param>
        /// <returns>Updated promo</returns>
        [HttpPost("update")]
        public async Task<ActionResult<PromoViewModel>> Update(UpdatePromoBody updatePromoBody)
        {
            try
            {
                Logger.LogInformation("Updating {Id}", updatePromoBody.Id);
                var result = await PromoService.Update(updatePromoBody);
                return result;
            }
            catch (Exception e)
            {
                Logger.LogInformation("Update exception {Exception}", e.ToString());
                return BadRequest(e);
            }
        }

        /// <summary>
        /// Endpoint for destroying a promo
        /// </summary>
        /// <param name="id">Promo id</param>
        /// <returns>Ok if destroyed</returns>
        [HttpDelete]
        public async Task<ActionResult> Destroy(Guid id)
        {
            try
            {
                Logger.LogInformation("Destroying {Id}", id);
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
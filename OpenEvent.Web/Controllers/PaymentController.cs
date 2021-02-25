using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenEvent.Web.Models.PaymentMethod;
using OpenEvent.Web.Services;

namespace OpenEvent.Web.Controllers
{
    /// <summary>
    /// API controller for all payment related endpoints.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService PaymentService;
        private readonly ILogger<PaymentController> Logger;
        
        public PaymentController(IPaymentService paymentService, ILogger<PaymentController> logger)
        {
            PaymentService = paymentService;
            Logger = logger;
        }

        /// <summary>
        /// Endpoint for adding a user's payment method.
        /// </summary>
        /// <param name="addPaymentMethodBody"></param>
        /// <returns></returns>
        [HttpPost("AddPaymentMethod")]
        public async Task<ActionResult<PaymentMethodViewModel>> AddPaymentMethod(AddPaymentMethodBody addPaymentMethodBody)
        {
            try
            {
                var result = await PaymentService.AddPaymentMethod(addPaymentMethodBody);
                return result;
            }
            catch (Exception e)
            {
                Logger.LogError(e.ToString());
                return BadRequest(e);
            }
        }
        
        /// <summary>
        /// Endpoint for removing a user's payment method.
        /// </summary>
        /// <param name="removePaymentMethodBody"></param>
        /// <returns></returns>
        [HttpPost("RemovePaymentMethod")]
        public async Task<ActionResult> RemovePaymentMethod(RemovePaymentMethodBody removePaymentMethodBody)
        {
            try
            {
                await PaymentService.RemovePaymentMethod(removePaymentMethodBody);
                return Ok();
            }
            catch (Exception e)
            {
                Logger.LogError(e.ToString());
                return BadRequest(e);
            }
        }
        
        /// <summary>
        /// Endpoint for setting the default payment method.
        /// </summary>
        /// <param name="makeDefaultBody"></param>
        /// <returns></returns>
        [HttpPost("MakePaymentDefault")]
        public async Task<ActionResult> MakePaymentDefault(MakeDefaultBody makeDefaultBody)
        {
            try
            {
                await PaymentService.MakeDefault(makeDefaultBody);
                return Ok();
            }
            catch (Exception e)
            {
                Logger.LogError(e.ToString());
                return BadRequest(e);
            }
        }
    }
}
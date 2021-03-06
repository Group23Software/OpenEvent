using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenEvent.Data.Models.PaymentMethod;
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

        /// <inheritdoc />
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
                Logger.LogInformation("{Id} is adding a payment method", addPaymentMethodBody.UserId);
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
                Logger.LogInformation("{Id} is removing a payment method", removePaymentMethodBody.UserId);
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
                Logger.LogInformation("{Id} is making a {Card} default", makeDefaultBody.UserId, makeDefaultBody.PaymentId);
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
using System;
using System.Threading.Tasks;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenEvent.Web.Models.PaymentMethod;
using OpenEvent.Web.Services;

namespace OpenEvent.Web.Controllers
{
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
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
        
        public PaymentController(IPaymentService userService, ILogger<PaymentController> logger)
        {
            PaymentService = userService;
            Logger = logger;
        }

        [HttpPost("AddPaymentMethod")]
        public async Task<ActionResult<PaymentMethodViewModel>> AddPaymentMethod(AddPaymentMethodModel addPaymentMethodModel)
        {
            try
            {
                var result = await PaymentService.AddPaymentMethod(addPaymentMethodModel);
                return result;
            }
            catch (Exception e)
            {
                Logger.LogError(e.ToString());
                return BadRequest(e);
            }
        }
    }
}
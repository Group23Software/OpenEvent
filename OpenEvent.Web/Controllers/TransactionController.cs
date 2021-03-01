using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenEvent.Web.Models.Intent;
using OpenEvent.Web.Models.Transaction;
using OpenEvent.Web.Services;
using Stripe;

namespace OpenEvent.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService TransactionService;
        private readonly ILogger<TransactionController> Logger;
        
        public TransactionController(ITransactionService transactionService, ILogger<TransactionController> logger)
        {
            TransactionService = transactionService;
            Logger = logger;
        }

        [HttpPost("CreateIntent")]
        public async Task<ActionResult<TransactionViewModel>> CreateIntent(CreateIntentBody createIntentBody)
        {
            try
            {
                var result = await TransactionService.CreateIntent(createIntentBody);
                return result;
            }
            catch (Exception e)
            {
                Logger.LogError(e.ToString());
                return BadRequest(e);
            }
        }

        [HttpPost("ConfirmIntent")]
        public async Task<ActionResult<TransactionViewModel>> ConfirmIntent(InjectPaymentMethodBody injectPaymentMethodBody)
        {
            try
            {
                var result = await TransactionService.InjectPaymentMethod(injectPaymentMethodBody);
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
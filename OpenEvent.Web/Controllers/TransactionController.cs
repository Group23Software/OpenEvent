using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenEvent.Data.Models.Intent;
using OpenEvent.Data.Models.Transaction;
using OpenEvent.Web.Services;
using Stripe;

namespace OpenEvent.Web.Controllers
{
    /// <inheritdoc />
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService TransactionService;
        private readonly ILogger<TransactionController> Logger;

        /// <inheritdoc />
        public TransactionController(ITransactionService transactionService, ILogger<TransactionController> logger)
        {
            TransactionService = transactionService;
            Logger = logger;
        }

        /// <summary>
        /// Endpoint for creating a new payment intent
        /// </summary>
        /// <param name="createIntentBody"><see cref="CreateIntentBody"/></param>
        /// <returns>New transaction</returns>
        [HttpPost("CreateIntent")]
        public async Task<ActionResult<TransactionViewModel>> CreateIntent(CreateIntentBody createIntentBody)
        {
            try
            {
                Logger.LogInformation("Creating intent of {Amount} by {User} for {Event}", createIntentBody.Amount, createIntentBody.UserId, createIntentBody.EventId);
                var result = await TransactionService.CreateIntent(createIntentBody);
                return result;
            }
            catch (Exception e)
            {
                Logger.LogError(e.ToString());
                return BadRequest(e);
            }
        }

        /// <summary>
        /// Endpoint for confirming a payment intent
        /// </summary>
        /// <param name="confirmIntentBody"><see cref="ConfirmIntentBody"/></param>
        /// <returns>Updated transaction</returns>
        [HttpPost("ConfirmIntent")]
        public async Task<ActionResult<TransactionViewModel>> ConfirmIntent(ConfirmIntentBody confirmIntentBody)
        {
            try
            {
                Logger.LogInformation("Confirming {Id} for {User}", confirmIntentBody.IntentId, confirmIntentBody.UserId);
                var result = await TransactionService.ConfirmIntent(confirmIntentBody);
                return result;
            }
            catch (Exception e)
            {
                Logger.LogError(e.ToString());
                return BadRequest(e);
            }
        }

        /// <summary>
        /// Endpoint for injecting a user's selected payment method into a payment
        /// </summary>
        /// <param name="injectPaymentMethodBody"><see cref="InjectPaymentMethodBody"/></param>
        /// <returns>Updated transaction</returns>
        [HttpPost("InjectPaymentMethod")]
        public async Task<ActionResult<TransactionViewModel>> InjectPaymentMethod(InjectPaymentMethodBody injectPaymentMethodBody)
        {
            try
            {
                Logger.LogInformation("Injecting payment into {Id}", injectPaymentMethodBody.IntentId);
                var result = await TransactionService.InjectPaymentMethod(injectPaymentMethodBody);
                return result;
            }
            catch (Exception e)
            {
                Logger.LogError(e.ToString());
                return BadRequest(e);
            }
        }

        /// <summary>
        /// Endpoint for canceling a payment intent
        /// </summary>
        /// <param name="cancelIntentBody"><see cref="CancelIntentBody"/></param>
        /// <returns>Ok if canceled</returns>
        [HttpPost("CancelIntent")]
        public async Task<ActionResult> CancelIntent(CancelIntentBody cancelIntentBody)
        {
            try
            {
                Logger.LogInformation("Canceling {Id}", cancelIntentBody.Id);
                await TransactionService.CancelIntent(cancelIntentBody);
                return Ok();
            }
            catch (Exception e)
            {
                Logger.LogError(e.ToString());
                return BadRequest(e);
            }
        }


        /// <summary>
        /// Endpoint for capturing the Stripe payment intent webhook 
        /// </summary>
        /// <returns>Ok</returns>
        [HttpPost("Capture")]
        public async Task<ActionResult> Capture()
        {
            Logger.LogInformation("Capturing webhook");

            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var stripeEvent = EventUtility.ParseEvent(json);

            await TransactionService.CaptureIntentHook(stripeEvent);

            return Ok();
        }
    }
}
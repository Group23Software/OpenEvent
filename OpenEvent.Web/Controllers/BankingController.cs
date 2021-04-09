using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenEvent.Data.Models.BankAccount;
using OpenEvent.Web.Services;

namespace OpenEvent.Web.Controllers
{
    /// <summary>
    /// API controller for all banking related endpoints.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class BankingController : ControllerBase
    {
        private readonly IBankingService BankingService;
        private readonly ILogger<BankingController> Logger;

        /// <inheritdoc />
        public BankingController(IBankingService bankingService, ILogger<BankingController> logger)
        {
            BankingService = bankingService;
            Logger = logger;
        }

        /// <summary>
        /// Endpoint for adding the user's bank account.
        /// </summary>
        /// <param name="addBankAccountBody"></param>
        /// <returns></returns>
        [HttpPost("AddBankAccount")]
        public async Task<ActionResult<BankAccountViewModel>> AddBankAccount(AddBankAccountBody addBankAccountBody)
        {
            try
            {
                Logger.LogInformation("{Id} is adding a bank", addBankAccountBody.UserId);
                var result = await BankingService.AddBankAccount(addBankAccountBody);
                return result;
            }
            catch (Exception e)
            {
                Logger.LogWarning(e.ToString());
                return BadRequest(e);
            }
        }

        /// <summary>
        /// Endpoint for removing the user's bank account.
        /// </summary>
        /// <param name="removeBankAccountBody"></param>
        /// <returns></returns>
        [HttpPost("RemoveBankAccount")]
        public async Task<ActionResult> RemoveBankAccount(RemoveBankAccountBody removeBankAccountBody)
        {
            try
            {
                Logger.LogInformation("{Id} is removing a bank", removeBankAccountBody.UserId);
                await BankingService.RemoveBankAccount(removeBankAccountBody);
                return Ok();
            }
            catch (Exception e)
            {
                Logger.LogWarning(e.ToString());
                return BadRequest(e);
            }
        }
    }
}
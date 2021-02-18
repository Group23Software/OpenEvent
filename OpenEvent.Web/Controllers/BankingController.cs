using System;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenEvent.Web.Models.BankAccount;
using OpenEvent.Web.Services;

namespace OpenEvent.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BankingController: ControllerBase
    {
        private readonly IBankingService BankingService;
        private readonly ILogger<BankingController> Logger;
        
        public BankingController(IBankingService bankingService, ILogger<BankingController> logger)
        {
            BankingService = bankingService;
            Logger = logger;
        }
        
        [HttpPost("AddBankAccount")]
        public async Task<ActionResult<BankAccountViewModel>> AddBankAccount(AddBankAccountBody addBankAccountBody)
        {
            try
            {
                var result = await BankingService.AddBankAccount(addBankAccountBody);
                return result;
            }
            catch (Exception e)
            {
                Logger.LogError(e.ToString());
                return BadRequest(e);
            }
        }
        
        [HttpPost("RemoveBankAccount")]
        public async Task<ActionResult> RemoveBankAccount(RemoveBankAccountBody removeBankAccountBody)
        {
            try
            {
                await BankingService.RemoveBankAccount(removeBankAccountBody);
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
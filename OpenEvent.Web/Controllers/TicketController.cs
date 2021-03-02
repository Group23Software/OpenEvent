using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenEvent.Web.Models.Ticket;
using OpenEvent.Web.Services;

namespace OpenEvent.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService TicketService;
        private readonly ILogger<TicketController> Logger;

        public TicketController(ITicketService ticketService, ILogger<TicketController> logger)
        {
            TicketService = ticketService;
            Logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<TicketDetailModel>> Get(Guid id)
        {
            try
            {
                var result = await TicketService.Get(id);
                return result;
            }
            catch (Exception e)
            {
                Logger.LogInformation(e.ToString());
                return BadRequest(e);
            }
        }

        [HttpGet("users")]
        public async Task<ActionResult<List<TicketViewModel>>> GetAllUsersTickets(Guid id)
        {
            try
            {
                var result = await TicketService.GetAllUsersTickets(id);
                return result;
            }
            catch (Exception e)
            {
                Logger.LogInformation(e.ToString());
                return BadRequest(e);
            }
        }

        [HttpPost("Verify")]
        public async Task<ActionResult> Verify(TicketVerifyBody ticketVerifyBody)
        {
            try
            {
                await TicketService.VerifyTicket(ticketVerifyBody);
                return Ok();
            }
            catch (Exception e)
            {
                Logger.LogInformation(e.ToString());
                return Unauthorized(e);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenEvent.Web.Models.Ticket;
using OpenEvent.Web.Services;

namespace OpenEvent.Web.Controllers
{
    /// <inheritdoc />
    [ApiController]
    [Route("api/[controller]")]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService TicketService;
        private readonly ILogger<TicketController> Logger;

        /// <inheritdoc />
        public TicketController(ITicketService ticketService, ILogger<TicketController> logger)
        {
            TicketService = ticketService;
            Logger = logger;
        }

        /// <summary>
        /// Endpoint for getting a ticket
        /// </summary>
        /// <param name="id">Ticket id</param>
        /// <returns>Ticket</returns>
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

        /// <summary>
        /// Endpoint for getting all a users tickets
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>List of tickets</returns>
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

        /// <summary>
        /// Endpoint for verifying a ticket
        /// </summary>
        /// <param name="ticketVerifyBody"><see cref="TicketVerifyBody"/></param>
        /// <returns>Ok if the ticket verifies and Unauthorized if not</returns>
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
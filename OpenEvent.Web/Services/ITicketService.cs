using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OpenEvent.Web.Models.Ticket;

namespace OpenEvent.Web.Services
{
    /// <summary>
    /// Service for all ticket logic
    /// </summary>
    public interface ITicketService
    {
        /// <summary>
        /// Get all the users tickets
        /// </summary>
        /// <param name="id">User's id</param>
        /// <returns>
        /// List of tickets
        /// </returns>
        Task<List<TicketViewModel>> GetAllUsersTickets(Guid id);

        /// <summary>
        /// Verifies the ticket
        /// </summary>
        /// <param name="ticketVerifyBody"><see cref="TicketVerifyBody"/></param>
        /// <returns>
        /// Completed task if verified
        /// </returns>
        Task VerifyTicket(TicketVerifyBody ticketVerifyBody);

        /// <summary>
        /// Gets a single ticket
        /// </summary>
        /// <param name="id">Ticket id</param>
        /// <returns>
        /// Ticket
        /// </returns>
        Task<TicketDetailModel> Get(Guid id);
    }
}
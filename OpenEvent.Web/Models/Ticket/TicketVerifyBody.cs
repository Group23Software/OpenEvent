using System;

namespace OpenEvent.Web.Models.Ticket
{
    /// <summary>
    /// Request body for verifying a ticket
    /// </summary>
    public class TicketVerifyBody
    {
        /// <summary>
        /// Ticket's id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Event's id
        /// </summary>
        public Guid EventId { get; set; }
    }
}
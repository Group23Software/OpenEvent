using System;

namespace OpenEvent.Data.Models.Intent
{
    /// <summary>
    /// Request body for canceling a payment intent
    /// </summary>
    public class CancelIntentBody
    {
        /// <summary>
        /// Transaction id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Event's id
        /// </summary>
        public Guid EventId { get; set; }

        /// <summary>
        /// Ticket's id
        /// </summary>
        public Guid TicketId { get; set; }
    }
}
using System;
using OpenEvent.Data.Models.Event;
using OpenEvent.Data.Models.Transaction;

namespace OpenEvent.Data.Models.Ticket
{
    /// <summary>
    /// Ticket detail model
    /// </summary>
    public class TicketDetailModel
    {
        /// <summary>
        /// Ticket id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// QRCode encoded as string
        /// </summary>
        public string QRCode { get; set; }
        
        /// <summary>
        /// Ticket's event
        /// </summary>
        public EventViewModel Event { get; set; }
        
        /// <summary>
        /// Ticket's transaction
        /// </summary>
        public TransactionViewModel Transaction { get; set; }
        
    }
}
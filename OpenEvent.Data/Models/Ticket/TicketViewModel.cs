using System;

namespace OpenEvent.Data.Models.Ticket
{
    /// <summary>
    /// Minimal ticket model.
    /// </summary>
    public class TicketViewModel
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
        /// Id of the ticket's event
        /// </summary>
        public Guid EventId { get; set; }

        /// <summary>
        /// name of event
        /// </summary>
        public string EventName { get; set; }

        /// <summary>
        /// When the event starts
        /// </summary>
        public DateTime EventStart { get; set; }

        /// <summary>
        /// When the event ends
        /// </summary>
        public DateTime EventEnd { get; set; }
    }
}
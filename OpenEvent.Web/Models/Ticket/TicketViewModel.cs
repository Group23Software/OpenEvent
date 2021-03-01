using System;

namespace OpenEvent.Web.Models.Ticket
{
    /// <summary>
    /// Minimal ticket model.
    /// </summary>
    public class TicketViewModel
    {
        public Guid Id { get; set; }
        public string QRCode { get; set; }
        
        public Guid EventId { get; set; }
        public string EventName { get; set; }
        public DateTime EventStart { get; set; }
        public DateTime EventEnd { get; set; }
    }
}
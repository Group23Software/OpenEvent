using System;

namespace OpenEvent.Web.Models.Intent
{
    public class CancelIntentBody
    {
        public string Id { get; set; }
        public Guid EventId { get; set; }
        public Guid TicketId { get; set; } 
    }
}
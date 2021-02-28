using System;

namespace OpenEvent.Web.Models.Intent
{
    public class CreateIntentBody
    {
        public Guid UserId { get; set; }
        public Guid EventId { get; set; }
        // public int NumberOfTickets { get; set; }
        public int Amount { get; set; }
    }
}
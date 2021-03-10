using System;

namespace OpenEvent.Web.Models.Ticket
{
    public class TicketVerifyBody
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
    }
}
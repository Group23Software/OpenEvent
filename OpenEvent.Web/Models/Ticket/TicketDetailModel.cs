using System;
using OpenEvent.Web.Models.Event;

namespace OpenEvent.Web.Models.Ticket
{
    public class TicketDetailModel
    {
        public Guid Id { get; set; }
        public string QRCode { get; set; }
        
        public EventViewModel Event { get; set; }
    }
}
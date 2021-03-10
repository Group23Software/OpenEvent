using System;
using OpenEvent.Web.Models.Event;
using OpenEvent.Web.Models.Transaction;

namespace OpenEvent.Web.Models.Ticket
{
    public class TicketDetailModel
    {
        public Guid Id { get; set; }
        public string QRCode { get; set; }
        
        public EventViewModel Event { get; set; }
        
        public TransactionViewModel Transaction { get; set; }
        
    }
}
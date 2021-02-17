using System;

namespace OpenEvent.Web.Models.Transaction
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public DateTime TimeOfTransaction { get; set; }
        public decimal Amount { get; set; }
        public bool Paid { get; set; }
        public string StripeChargeId { get; set; }
        
        // TODO: Does this need both sides of the transaction host and user
        public User.User User { get; set; }
        
        public Guid TicketId { get; set; }
        public Ticket.Ticket Ticket { get; set; }
    }
}
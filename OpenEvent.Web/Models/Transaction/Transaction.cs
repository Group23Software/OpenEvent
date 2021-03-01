using System;
using System.ComponentModel.DataAnnotations;
using OpenEvent.Web.Models.Ticket;
using OpenEvent.Web.Models.User;

namespace OpenEvent.Web.Models.Transaction
{
    public class Transaction
    {
        [Key]
        public string StripeIntentId { get; set; }
        public DateTime Start { get; set; }
        public DateTime Updated { get; set; }
        public DateTime End { get; set; }
        public long Amount { get; set; }
        public bool Paid { get; set; }
        public PaymentStatus Status { get; set; }

        // TODO: Does this need both sides of the transaction host and user
        public User.User User { get; set; }
        
        public Guid TicketId { get; set; }
        public Ticket.Ticket Ticket { get; set; }
    }

    public class TransactionViewModel
    {
        public string StripeIntentId { get; set; }
        public DateTime Start { get; set; }
        public DateTime Updated { get; set; }
        public DateTime End { get; set; }
        public long Amount { get; set; }
        public bool Paid { get; set; }
        public PaymentStatus Status { get; set; }
        public Guid TicketId { get; set; }
    }
}
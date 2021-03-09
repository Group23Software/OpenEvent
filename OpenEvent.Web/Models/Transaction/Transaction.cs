using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

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
        [JsonIgnore] public User.User User { get; set; }
        
        public Guid TicketId { get; set; }
        [JsonIgnore] public Ticket.Ticket Ticket { get; set; }
        [JsonIgnore] public Event.Event Event { get; set; }
        
        [NotMapped] public string ClientSecret { get; set; }
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
        public string ClientSecret { get; set; }
    }
}
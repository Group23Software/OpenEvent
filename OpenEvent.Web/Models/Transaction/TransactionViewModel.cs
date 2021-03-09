using System;

namespace OpenEvent.Web.Models.Transaction
{
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
        public Guid EventId { get; set; }
    }
}
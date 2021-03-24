using System;

namespace OpenEvent.Web.Models.Transaction
{
    /// <summary>
    /// Transaction view model
    /// </summary>
    public class TransactionViewModel
    {
        /// <summary>
        /// Stripe intent id of this transaction
        /// </summary>
        public string StripeIntentId { get; set; }

        /// <summary>
        /// When the intent was created
        /// </summary>
        public DateTime Start { get; set; }

        /// <summary>
        /// Last time the intent was updated
        /// </summary>
        public DateTime Updated { get; set; }

        /// <summary>
        /// Set when the intent is successful or canceled
        /// </summary>
        public DateTime End { get; set; }

        /// <summary>
        /// Amount of money
        /// </summary>
        public long Amount { get; set; }

        /// <summary>
        /// If the payment was successful
        /// </summary>
        public bool Paid { get; set; }

        /// <summary>
        /// Current status of the payment intent
        /// </summary>
        public PaymentStatus Status { get; set; }

        /// <summary>
        /// Id of the ticket paid for
        /// </summary>
        public Guid TicketId { get; set; }

        /// <summary>
        /// Secret intent token sent to client once on creation and never saved or sent back to server
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// Event id associated
        /// </summary>
        public Guid EventId { get; set; }

        /// <summary>
        /// If a promo if has been used
        /// </summary>
        public Guid PromoId { get; set; }
    }
}
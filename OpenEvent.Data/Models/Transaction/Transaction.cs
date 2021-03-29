using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace OpenEvent.Data.Models.Transaction
{
    /// <summary>
    /// Base transaction model
    /// </summary>
    public class Transaction
    {
        /// <summary>
        /// Stripe intent id of this transaction
        /// </summary>
        [Key]
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

        // TODO: Does this need both sides of the transaction host and user
        /// <summary>
        /// User who pays the transaction
        /// </summary>
        [JsonIgnore]
        public User.User User { get; set; }

        /// <summary>
        /// Id of the ticket paid for
        /// </summary>
        public Guid TicketId { get; set; }

        /// <summary>
        /// Ticket paid for
        /// </summary>
        [JsonIgnore]
        public Ticket.Ticket Ticket { get; set; }

        /// <summary>
        /// Event associated
        /// </summary>
        [JsonIgnore]
        public Event.Event Event { get; set; }

        /// <summary>
        /// If a promo if has been used
        /// </summary>
        public Guid? PromoId { get; set; }

        /// <summary>
        /// Secret intent token sent to client once on creation and never saved or sent back to server
        /// </summary>
        [NotMapped]
        public string ClientSecret { get; set; }
    }
}
using System;

namespace OpenEvent.Web.Models.Intent
{
    /// <summary>
    /// Request body for injecting a payment method
    /// </summary>
    public class InjectPaymentMethodBody
    {
        /// <summary>
        /// User's id
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Stripe intent id
        /// </summary>
        public string IntentId { get; set; }

        /// <summary>
        /// Stripe card id
        /// </summary>
        public string CardId { get; set; }
    }
}
using System;

namespace OpenEvent.Web.Models.Intent
{
    /// <summary>
    /// Request body for confirming an intent
    /// </summary>
    public class ConfirmIntentBody
    {
        /// <summary>
        /// User's id
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Stripe intent id
        /// </summary>
        public string IntentId { get; set; }
    }
}
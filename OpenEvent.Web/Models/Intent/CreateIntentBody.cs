using System;

namespace OpenEvent.Web.Models.Intent
{
    /// <summary>
    /// Request body for creating an intent
    /// </summary>
    public class CreateIntentBody
    {
        /// <summary>
        /// User's id
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Event's id
        /// </summary>
        public Guid EventId { get; set; }

        /// <summary>
        /// Amount/price of intent eg: 1050
        /// </summary>
        public int Amount { get; set; }
    }
}
using System;

namespace OpenEvent.Data.Models.PaymentMethod
{
    /// <summary>
    /// Request body to make a payment method default
    /// </summary>
    public class MakeDefaultBody
    {
        /// <summary>
        /// User's id
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Payment method id
        /// </summary>
        public string PaymentId { get; set; }
    }
}
using System;

namespace OpenEvent.Data.Models.PaymentMethod
{
    /// <summary>
    /// Request body for removing a payment method
    /// </summary>
    public class RemovePaymentMethodBody
    {
        /// <summary>
        /// Id of user who owns method
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Payment method id
        /// </summary>
        public string PaymentId { get; set; }
    }
}
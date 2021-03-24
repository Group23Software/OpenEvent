namespace OpenEvent.Web.Models.Transaction
{
    /// <summary>
    /// Copied from Stripe api into enum, all possible statuses of a payment intent
    /// </summary>
    public enum PaymentStatus
    {
        /// <summary>
        /// Canceled/Failed
        /// </summary>
        canceled,

        /// <summary>
        /// In progress
        /// </summary>
        processing,

        /// <summary>
        /// Requires action eg: 3D secure
        /// </summary>
        requires_action,

        /// <summary>
        /// Requires capture, shouldn't get this status as auto capture is set
        /// </summary>
        requires_capture,

        /// <summary>
        /// Requires user confirmation, final step of payment
        /// </summary>
        requires_confirmation,

        /// <summary>
        /// Needs a payment method
        /// </summary>
        requires_payment_method,

        /// <summary>
        /// Successfully paid
        /// </summary>
        succeeded
    }
}
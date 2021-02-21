using System;

namespace OpenEvent.Web.Exceptions
{
    /// <summary>
    /// Payment method not found exception.
    /// </summary>
    public class PaymentMethodNotFoundException : Exception
    {
        /// <inheritdoc />
        public PaymentMethodNotFoundException(): base("Payment method not found")
        {
        }
    }
}
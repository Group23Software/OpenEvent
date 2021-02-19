using System;

namespace OpenEvent.Web.Exceptions
{
    public class PaymentMethodNotFoundException : Exception
    {
        public PaymentMethodNotFoundException(): base("Payment method not found")
        {
        }
    }
}
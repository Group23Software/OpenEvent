using System;

namespace OpenEvent.Web.Models.PaymentMethod
{
    public class RemovePaymentMethodBody
    {
        public Guid UserId { get; set; }
        public string PaymentId { get; set; }
    }
}
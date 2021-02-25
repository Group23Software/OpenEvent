using System;

namespace OpenEvent.Web.Models.PaymentMethod
{
    public class MakeDefaultBody
    {
        public Guid UserId { get; set; }
        public string PaymentId { get; set; }
    }
}
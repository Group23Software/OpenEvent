using System;

namespace OpenEvent.Web.Models.Intent
{
    public class InjectPaymentMethodBody
    {
        public Guid UserId { get; set; }
        public string IntentId { get; set; }
        public string CardId { get; set; }
    }
}
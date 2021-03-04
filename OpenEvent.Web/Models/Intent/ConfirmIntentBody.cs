using System;

namespace OpenEvent.Web.Models.Intent
{
    public class ConfirmIntentBody
    {
        public Guid UserId { get; set; }
        public string IntentId { get; set; }
    }
}
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using OpenEvent.Web.Models.Analytic;

namespace OpenEvent.Web.Models.Ticket
{
    /// <summary>
    /// Ticket model.
    /// </summary>
    public class Ticket
    {
        public Guid Id { get; set; }
        public byte[] QRCode { get; set; }
        [JsonIgnore] public Event.Event Event { get; set; }
        [JsonIgnore] public User.User User { get; set; }
        public Transaction.Transaction Transaction { get; set; }
        public bool Available { get; set; }
        public int Uses { get; set; }
        [JsonIgnore] public List<TicketVerificationEvent> VerificationEvents { get; set; }
    }

    public class TicketVerifyBody
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
    }
}
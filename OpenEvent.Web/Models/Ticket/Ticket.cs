using System;
using Newtonsoft.Json;

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
    }
}
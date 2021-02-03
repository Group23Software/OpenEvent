using System;

namespace OpenEvent.Web.Models.Ticket
{
    /// <summary>
    /// Ticket model.
    /// </summary>
    public class Ticket
    {
        public Guid Id { get; set; }
        public byte[] QRCode { get; set; }
        
        public Event.Event Event { get; set; }
        public User.User User { get; set; }
        // TODO: Transaction
    }
}
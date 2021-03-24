using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using OpenEvent.Web.Models.Analytic;

namespace OpenEvent.Web.Models.Ticket
{
    /// <summary>
    /// Base ticket model.
    /// </summary>
    public class Ticket
    {
        /// <summary>
        /// Unique id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// QRCode bitmap image represented as a byte array
        /// </summary>
        public byte[] QRCode { get; set; }

        /// <summary>
        /// Event the ticket is for
        /// </summary>
        [JsonIgnore]
        public Event.Event Event { get; set; }

        /// <summary>
        /// User who owns the ticket
        /// </summary>
        [JsonIgnore]
        public User.User User { get; set; }

        /// <summary>
        /// Transaction associated to the ticket
        /// </summary>
        public Transaction.Transaction Transaction { get; set; }

        /// <summary>
        /// If the ticket is available to purchase
        /// </summary>
        public bool Available { get; set; }

        /// <summary>
        /// Number of times the ticket has been verified
        /// </summary>
        public int Uses { get; set; }

        /// <summary>
        /// verification events associated with the ticket
        /// </summary>
        [JsonIgnore]
        public List<TicketVerificationEvent> VerificationEvents { get; set; }
    }
}
using System;
using Newtonsoft.Json;

namespace OpenEvent.Web.Models.Analytic
{
    /// <summary>
    /// Ticket verification event analytic
    /// </summary>
    public class TicketVerificationEvent : AnalyticEvent
    {
        /// <inheritdoc />
        public override Guid Id { get; set; }

        /// <inheritdoc />
        public override DateTime Created { get; set; }

        /// <summary>
        /// Ticket verified 
        /// </summary>
        [JsonIgnore]
        public Ticket.Ticket Ticket { get; set; }

        /// <summary>
        /// User who owns the ticket
        /// </summary>
        [JsonIgnore]
        public User.User User { get; set; }

        /// <summary>
        /// Event the ticket is for
        /// </summary>
        [JsonIgnore]
        public Event.Event Event { get; set; }
    }
}
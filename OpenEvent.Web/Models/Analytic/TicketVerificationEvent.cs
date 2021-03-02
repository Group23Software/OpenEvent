using System;
using Newtonsoft.Json;

namespace OpenEvent.Web.Models.Analytic
{
    public class TicketVerificationEvent: AnalyticEvent
    {
        public override Guid Id { get; set; }
        public override DateTime Created { get; set; }
        [JsonIgnore] public Ticket.Ticket Ticket { get; set; }
        [JsonIgnore] public User.User User { get; set; }
        [JsonIgnore] public Event.Event Event { get; set; }
    }
}
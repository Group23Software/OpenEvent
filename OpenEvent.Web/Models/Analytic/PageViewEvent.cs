using System;
using Newtonsoft.Json;

namespace OpenEvent.Web.Models.Analytic
{
    public class PageViewEvent : AnalyticEvent
    {
        public override Guid Id { get; set; }
        public override DateTime Created { get; set; }
        [JsonIgnore] public Event.Event Event { get; set; }
        [JsonIgnore] public User.User User { get; set; }
    }

    public class PageViewEventViewModel : AnalyticEvent
    {
        public override Guid Id { get; set; }
        public override DateTime Created { get; set; }
        public Guid EventId { get; set; }
        public Guid UserId { get; set; }
    }
}
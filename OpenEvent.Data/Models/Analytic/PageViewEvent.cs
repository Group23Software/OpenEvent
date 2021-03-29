using System;
using Newtonsoft.Json;

namespace OpenEvent.Data.Models.Analytic
{
    /// <summary>
    /// Page view event analytic
    /// </summary>
    public class PageViewEvent : AnalyticEvent
    {
        /// <inheritdoc />
        public override Guid Id { get; set; }

        /// <inheritdoc />
        public override DateTime Created { get; set; }

        /// <summary>
        /// Event viewed
        /// </summary>
        [JsonIgnore]
        public Event.Event Event { get; set; }

        /// <summary>
        /// User who viewed the event
        /// </summary>
        [JsonIgnore]
        public User.User User { get; set; }
    }
}
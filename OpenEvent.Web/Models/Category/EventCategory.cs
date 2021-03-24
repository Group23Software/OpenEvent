using System;
using Newtonsoft.Json;

namespace OpenEvent.Web.Models.Category
{
    /// <summary>
    /// Junction table for events and categories.
    /// </summary>
    public class EventCategory
    {
        /// <summary>
        /// Categories id
        /// </summary>
        public Guid CategoryId { get; set; }

        /// <summary>
        /// Category
        /// </summary>
        public Category Category { get; set; }

        /// <summary>
        /// Event's id
        /// </summary>
        public Guid EventId { get; set; }

        /// <summary>
        /// Event
        /// </summary>
        [JsonIgnore]
        public Event.Event Event { get; set; }
    }
}
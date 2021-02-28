using System;
using Newtonsoft.Json;

namespace OpenEvent.Web.Models.Category
{
    /// <summary>
    /// Junction table for events and categories.
    /// </summary>
    public class EventCategory
    {
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
        public Guid EventId { get; set; }
        [JsonIgnore] public Event.Event Event { get; set; }
    }
}
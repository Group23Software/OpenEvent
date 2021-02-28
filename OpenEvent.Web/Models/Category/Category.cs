using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using OpenEvent.Web.Models.Recommendation;

namespace OpenEvent.Web.Models.Category
{
    /// <summary>
    /// Category for event.
    /// </summary>
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        [JsonIgnore] public List<EventCategory> Events { get; set; }
        [JsonIgnore] public List<RecommendationScore> Scores { get; set; }
    }
}
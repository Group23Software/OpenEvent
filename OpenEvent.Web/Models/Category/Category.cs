using System;
using System.Collections.Generic;
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
        public List<EventCategory> Events { get; set; }
        public List<RecommendationScore> Scores { get; set; }
    }
}
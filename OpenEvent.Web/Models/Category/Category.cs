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
        /// <summary>
        /// Unique id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Name of category eg: Music
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// List of all event categories with this category
        /// </summary>
        [JsonIgnore]
        public List<EventCategory> Events { get; set; }

        /// <summary>
        /// List of recommendation scores of this category
        /// </summary>
        [JsonIgnore]
        public List<RecommendationScore> Scores { get; set; }
    }
}
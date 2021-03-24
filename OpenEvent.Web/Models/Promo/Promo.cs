using System;
using Newtonsoft.Json;

namespace OpenEvent.Web.Models.Promo
{
    /// <summary>
    /// Base promo model
    /// </summary>
    public class Promo
    {
        /// <summary>
        /// Unique id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Start date and time of promo
        /// </summary>
        public DateTime Start { get; set; }

        /// <summary>
        /// End date and time of promo 
        /// </summary>
        public DateTime End { get; set; }

        /// <summary>
        /// Percentage discount applied to price
        /// </summary>
        public int Discount { get; set; }

        /// <summary>
        /// If the promo is active
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Event the promo is for
        /// </summary>
        [JsonIgnore]
        public Event.Event Event { get; set; }
    }
}
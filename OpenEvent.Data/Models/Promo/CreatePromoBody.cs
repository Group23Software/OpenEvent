using System;

namespace OpenEvent.Data.Models.Promo
{
    /// <summary>
    /// Request body for creating a promo
    /// </summary>
    public class CreatePromoBody
    {
        /// <summary>
        /// Start date and time of promo
        /// </summary>
        public DateTime Start { get; set; }

        /// <summary>
        /// End date and time of promo 
        /// </summary>
        public DateTime End { get; set; }

        /// <summary>
        /// If the promo is active
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Percentage discount applied to price
        /// </summary>
        public int Discount { get; set; }

        /// <summary>
        /// Id of event promo is made for
        /// </summary>
        public Guid EventId { get; set; }
    }
}
using System;

namespace OpenEvent.Data.Models.Promo
{
    /// <summary>
    /// Request body for updating a promo
    /// </summary>
    public class UpdatePromoBody
    {
        /// <summary>
        /// Promo's id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Start date
        /// </summary>
        public DateTime Start { get; set; }

        /// <summary>
        /// End date
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
    }
}
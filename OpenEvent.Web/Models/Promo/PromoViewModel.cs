using System;

namespace OpenEvent.Web.Models.Promo
{
    /// <summary>
    /// Promo view model
    /// </summary>
    public class PromoViewModel
    {
        /// <summary>
        /// Promo's id
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
        /// Number of transactions that have used this promo
        /// </summary>
        public int NumberOfSales { get; set; }
    }
}
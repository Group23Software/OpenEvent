using System;

namespace OpenEvent.Web.Models.Recommendation
{
    /// <summary>
    /// Recommendation score view model
    /// </summary>
    public class RecommendationScoreViewModel
    {
        /// <summary>
        /// RecommendationScore's id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Name of category
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// User's weight towards the category
        /// </summary>
        public double Weight { get; set; }
    }
}
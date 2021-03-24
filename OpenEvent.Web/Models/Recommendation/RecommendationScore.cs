using System;

namespace OpenEvent.Web.Models.Recommendation
{
    /// <summary>
    /// Base recommendation score model
    /// </summary>
    public class RecommendationScore
    {
        /// <summary>
        /// Unique id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// User who owns the score
        /// </summary>
        public User.User User { get; set; }

        /// <summary>
        /// Category is being scored
        /// </summary>
        public Category.Category Category { get; set; }

        /// <summary>
        /// Weight representing the users preference to the category
        /// </summary>
        public double Weight { get; set; }
    }
}
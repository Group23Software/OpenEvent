using System.Collections.Generic;
using OpenEvent.Data.Models.Recommendation;

namespace OpenEvent.Data.Models.Analytic
{
    /// <summary>
    /// All analytics associated with the user
    /// </summary>
    public class UsersAnalytics
    {
        /// <summary>
        /// Page views
        /// </summary>
        public List<PageViewEventViewModel> PageViewEvents { get; set; }

        /// <summary>
        /// Searches
        /// </summary>
        public List<SearchEventViewModel> SearchEvents { get; set; }

        /// <summary>
        /// Recommendation scores
        /// </summary>
        public List<RecommendationScoreViewModel> RecommendationScores { get; set; }

        /// <summary>
        /// Ticket verifications
        /// </summary>
        public List<TicketVerificationEventViewModel> TicketVerificationEvents { get; set; }
    }
}
using System.Collections.Generic;
using OpenEvent.Web.Models.Recommendation;

namespace OpenEvent.Web.Models.Analytic
{
    public class UsersAnalytics
    {
        public List<PageViewEventViewModel> PageViewEvents { get; set; }
        public List<SearchEventViewModel> SearchEvents { get; set; }
        public List<RecommendationScoreViewModel> RecommendationScores { get; set; }
        public List<TicketVerificationEventViewModel> TicketVerificationEvents { get; set; } 
    }
}
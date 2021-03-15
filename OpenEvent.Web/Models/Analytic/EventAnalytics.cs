using System;
using System.Collections.Generic;
using OpenEvent.Web.Models.Recommendation;

namespace OpenEvent.Web.Models.Analytic
{
    public class EventAnalytics
    {
        public List<PageViewEventViewModel> PageViewEvents { get; set; }
        public List<TicketVerificationEventViewModel> TicketVerificationEvents { get; set; }
        public List<AverageRecommendationScore> AverageRecommendationScores { get; set; }
        public List<Demographic> Demographics { get; set; }
    }
    
    public class AverageRecommendationScore
    {
        public String CategoryName { get; set; }
        public double Weight { get; set; }
    }
}
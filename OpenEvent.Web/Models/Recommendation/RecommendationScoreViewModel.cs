using System;

namespace OpenEvent.Web.Models.Recommendation
{
    public class RecommendationScoreViewModel
    {
        public Guid Id { get; set; }
        public String CategoryName { get; set; }
        public double Weight { get; set; }
    }
}
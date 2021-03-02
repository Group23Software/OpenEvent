using System;

namespace OpenEvent.Web.Models.Recommendation
{
    public class RecommendationScore
    {
        public Guid Id { get; set; }
        public User.User User { get; set; }
        
        public Category.Category Category { get; set; }
        public double Weight { get; set; }
    }
    
    public class RecommendationScoreViewModel
    {
        public Guid Id { get; set; }
        public String CategoryName { get; set; }
        public double Weight { get; set; }
    }
}
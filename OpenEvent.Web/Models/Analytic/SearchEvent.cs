using System;

namespace OpenEvent.Web.Models.Analytic
{
    public class SearchEvent : AnalyticEvent
    {
        public override Guid Id { get; set; }
        public override DateTime Created { get; set; }
        public User.User User { get; set; }
        public string Search { get; set; }
        public string Params { get; set; }
    }
    
    public class SearchEventViewModel: AnalyticEvent
    {
        public override Guid Id { get; set; }
        public override DateTime Created { get; set; }
        public Guid UserId { get; set; }
        public string Search { get; set; }
        public string Params { get; set; }
    }
}
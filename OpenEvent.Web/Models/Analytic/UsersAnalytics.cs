using System.Collections.Generic;

namespace OpenEvent.Web.Models.Analytic
{
    public class UsersAnalytics
    {
        public List<PageViewEventViewModel> PageViewEvents { get; set; }
        public List<SearchEventViewModel> SearchEvents { get; set; }
    }
}
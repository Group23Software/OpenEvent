using System.Collections.Generic;

namespace OpenEvent.Data.Models.Analytic
{
    /// <summary>
    /// All analytics associated with an event
    /// </summary>
    public class EventAnalytics
    {
        /// <summary>
        /// List of all page views
        /// </summary>
        public List<PageViewEventViewModel> PageViewEvents { get; set; }

        /// <summary>
        /// list of all ticket verification events
        /// </summary>
        public List<TicketVerificationEventViewModel> TicketVerificationEvents { get; set; }

        /// <summary>
        /// List of Demographics based on page view analytics
        /// </summary>
        public List<Demographic> Demographics { get; set; }
    }
}
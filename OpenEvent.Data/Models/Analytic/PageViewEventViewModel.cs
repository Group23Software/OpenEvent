using System;

namespace OpenEvent.Data.Models.Analytic
{
    /// <summary>
    /// View model for page views
    /// </summary>
    public class PageViewEventViewModel : AnalyticEvent
    {
        /// <inheritdoc />
        public override Guid Id { get; set; }

        /// <inheritdoc />
        public override DateTime Created { get; set; }

        /// <summary>
        /// Id of event viewed
        /// </summary>
        public Guid EventId { get; set; }

        /// <summary>
        /// Id of the user who viewed the event
        /// </summary>
        public Guid UserId { get; set; }
    }
}
using System;

namespace OpenEvent.Web.Models.Analytic
{
    /// <summary>
    /// View model for search event
    /// </summary>
    public class SearchEventViewModel : AnalyticEvent
    {
        /// <inheritdoc />
        public override Guid Id { get; set; }

        /// <inheritdoc />
        public override DateTime Created { get; set; }

        /// <summary>
        /// Id of user who searched
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Search keyword
        /// </summary>
        public string Search { get; set; }

        /// <summary>
        /// Search filters concatenated into one string
        /// </summary>
        public string Params { get; set; }
    }
}
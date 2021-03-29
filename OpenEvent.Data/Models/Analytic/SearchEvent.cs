using System;

namespace OpenEvent.Data.Models.Analytic
{
    /// <summary>
    /// Search event analytic
    /// </summary>
    public class SearchEvent : AnalyticEvent
    {
        /// <inheritdoc />
        public override Guid Id { get; set; }

        /// <inheritdoc />
        public override DateTime Created { get; set; }

        /// <summary>
        /// User who searched
        /// </summary>
        public User.User User { get; set; }

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
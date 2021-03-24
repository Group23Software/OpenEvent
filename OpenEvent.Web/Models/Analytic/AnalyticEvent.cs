using System;

namespace OpenEvent.Web.Models.Analytic
{
    /// <summary>
    /// Base class for all analytic events
    /// </summary>
    public abstract class AnalyticEvent
    {
        /// <summary>
        /// Unique id
        /// </summary>
        public abstract Guid Id { get; set; }

        /// <summary>
        /// Time of creation
        /// </summary>
        public abstract DateTime Created { get; set; }
    }
}
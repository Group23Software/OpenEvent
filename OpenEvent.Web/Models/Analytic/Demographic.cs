using System;

namespace OpenEvent.Web.Models.Analytic
{
    /// <summary>
    /// Demographic object used when getting event analytics
    /// </summary>
    public class Demographic
    {
        /// <summary>
        /// Unique age
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// Current count
        /// </summary>
        public int Count { get; set; }
    }
}
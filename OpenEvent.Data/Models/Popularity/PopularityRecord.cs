using System;

namespace OpenEvent.Data.Models.Popularity
{
    /// <summary>
    /// Record of a popular entity
    /// </summary>
    public class PopularityRecord
    {
        /// <summary>
        /// Id of entity
        /// </summary>
        public Guid Record { get; set; }

        /// <summary>
        /// Date and time the record was created
        /// </summary>
        public DateTime Added { get; set; }

        /// <summary>
        /// Date and time the record was last updated
        /// </summary>
        public DateTime Updated { get; set; }

        /// <summary>
        /// Number of page views in the recent past
        /// </summary>
        public int Score { get; set; }
    }
}
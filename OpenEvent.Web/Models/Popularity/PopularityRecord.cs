using System;

namespace OpenEvent.Web.Models.Popularity
{
    public class PopularityRecord<T>
    {
        public T Record { get; set; }
        public DateTime Added { get; set; }
        public DateTime Updated { get; set; }
        // Number of records over time
        public int Score { get; set; }
    }
}
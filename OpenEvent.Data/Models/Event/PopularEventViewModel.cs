namespace OpenEvent.Data.Models.Event
{
    /// <summary>
    /// Extension of EventViewModel with score
    /// </summary>
    public class PopularEventViewModel : EventViewModel
    {
        /// <summary>
        /// Score/number of time the event has been viewed in the recent past 
        /// </summary>
        public int Score { get; set; }
    }
}
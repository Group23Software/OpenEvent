namespace OpenEvent.Web.Models.Event
{
    /// <summary>
    /// Possible search filters
    /// </summary>
    public enum SearchParam
    {
        /// <summary>
        /// Event category
        /// </summary>
        Category,

        /// <summary>
        /// Within range of location
        /// </summary>
        Location,

        /// <summary>
        /// If event is online
        /// </summary>
        IsOnline,

        /// <summary>
        /// Time of event
        /// </summary>
        Date
    }
}
namespace OpenEvent.Data.Models.Recommendation
{
    /// <summary>
    /// Amount of influence different events inflict on the user's recommendation score
    /// </summary>
    public enum Influence
    {
        /// <summary>
        /// Simple page view event, has lowest influence
        /// </summary>
        PageView = 200,
        /// <summary>
        /// Search containing the category
        /// </summary>
        Search = 100,
        /// <summary>
        /// Ticket purchase has highest priority
        /// </summary>
        Purchase = 500,
        /// <summary>
        /// Ticket verification
        /// </summary>
        Verify = 400,
        /// <summary>
        /// Down vote decreased the user's recommendation score 
        /// </summary>
        DownVote = -100
    }
}
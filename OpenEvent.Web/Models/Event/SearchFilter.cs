namespace OpenEvent.Web.Models.Event
{
    /// <summary>
    /// Search filter as key value pair
    /// </summary>
    public class SearchFilter
    {
        /// <summary>
        /// Type of filter
        /// </summary>
        public SearchParam Key { get; set; }

        /// <summary>
        /// filter value eg categoryId
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Converts filter into string, for unique cache key
        /// </summary>
        /// <returns>String of key and value</returns>
        public override string ToString()
        {
            return $"{Key}:{Value}";
        }
    }
}
namespace OpenEvent.Data.Models.Category
{
    /// <summary>
    /// Extension of CategoryViewModel with score
    /// </summary>
    public class PopularCategoryViewModel : CategoryViewModel
    {
        /// <summary>
        /// Score/number of time the category has been viewed in the recent past 
        /// </summary>
        public int Score { get; set; }
    }
}
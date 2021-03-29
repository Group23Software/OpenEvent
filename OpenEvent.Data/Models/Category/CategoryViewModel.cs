using System;

namespace OpenEvent.Data.Models.Category
{
    /// <summary>
    /// Minimal category.
    /// </summary>
    public class CategoryViewModel
    {
        /// <summary>
        /// Category's id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Name of category eg: Music
        /// </summary>
        public string Name { get; set; }
    }
}
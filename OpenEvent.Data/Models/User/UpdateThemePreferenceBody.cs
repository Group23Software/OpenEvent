using System;

namespace OpenEvent.Data.Models.User
{
    /// <summary>
    /// Request body for updating a user's theme preference 
    /// </summary>
    public class UpdateThemePreferenceBody
    {
        /// <summary>
        /// If the user prefers dark mode
        /// </summary>
        public bool IsDarkMode { get; set; }

        /// <summary>
        /// User's id
        /// </summary>
        public Guid Id { get; set; }
    }
}
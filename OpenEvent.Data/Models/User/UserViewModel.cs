using System;

namespace OpenEvent.Data.Models.User
{
    /// <summary>
    /// Minimal user data.
    /// </summary>
    public class UserViewModel
    {
        /// <summary>
        /// User's id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Unique username
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Avatar bitmap image encoded as string
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// Jwt token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// If the user prefers dark mode
        /// </summary>
        public bool IsDarkMode { get; set; }
    }
}
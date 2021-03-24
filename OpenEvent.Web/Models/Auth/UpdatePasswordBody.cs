using System;

namespace OpenEvent.Web.Models.Auth
{
    /// <summary>
    /// Request body for updating user's password.
    /// </summary>
    public class UpdatePasswordBody
    {
        /// <summary>
        /// User's id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// User's password
        /// </summary>
        public string Password { get; set; }
    }
}
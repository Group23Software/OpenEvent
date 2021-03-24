using System;

namespace OpenEvent.Web.Models.User
{
    /// <summary>
    ///  Request body for updating user's username.
    /// </summary>
    public class UpdateUserNameBody
    {
        /// <summary>
        /// User's id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// New unique username
        /// </summary>
        public string UserName { get; set; }
    }
}
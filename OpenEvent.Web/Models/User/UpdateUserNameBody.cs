using System;

namespace OpenEvent.Web.Models.User
{
    /// <summary>
    ///  Request body for updating user's username.
    /// </summary>
    public class UpdateUserNameBody
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
    }
}
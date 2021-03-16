using System;

namespace OpenEvent.Web.Models.Auth
{
    /// <summary>
    /// Request body for updating user's password.
    /// </summary>
    public class UpdatePasswordBody
    {
        public Guid Id { get; set; }
        public string Password { get; set; }
    }
}
using System;

namespace OpenEvent.Web.Models.Auth
{
    /// <summary>
    /// Request body for authenticating.
    /// </summary>
    public class AuthBody
    {
        /// <summary>
        /// User id
        /// </summary>
        public Guid Id { get; set; }
    }
}
using System;

namespace OpenEvent.Data.Models.Auth
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
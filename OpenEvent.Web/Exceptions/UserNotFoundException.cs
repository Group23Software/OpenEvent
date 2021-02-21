using System;

namespace OpenEvent.Web.Exceptions
{
    /// <summary>
    /// User not found exception.
    /// </summary>
    [Serializable]
    public class UserNotFoundException : Exception
    {
        /// <inheritdoc />
        public UserNotFoundException() : base("User Not Found")
        {
        }
    }
}
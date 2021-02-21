using System;

namespace OpenEvent.Web.Exceptions
{
    /// <summary>
    /// User already exists exception.
    /// </summary>
    [Serializable]
    public class UserAlreadyExistsException : Exception
    {
        /// <inheritdoc />
        public UserAlreadyExistsException() : base("User already exists")
        {
        }
    }
}
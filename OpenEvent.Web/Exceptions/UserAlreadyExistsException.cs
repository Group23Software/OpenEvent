using System;

namespace OpenEvent.Web.Exceptions
{
    /// <summary>
    /// User already exists exception.
    /// </summary>
    [Serializable]
    public class UserAlreadyExistsException : Exception
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public UserAlreadyExistsException() : base("User already exists")
        {
        }
    }
}
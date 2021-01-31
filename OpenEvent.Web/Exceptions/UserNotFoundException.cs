using System;

namespace OpenEvent.Web.Exceptions
{
    /// <summary>
    /// User not found exception.
    /// </summary>
    [Serializable]
    public class UserNotFoundException : Exception
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public UserNotFoundException() : base("User Not Found")
        {
        }
    }
}
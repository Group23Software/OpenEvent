using System;

namespace OpenEvent.Web.Exceptions
{
    /// <summary>
    /// Username already exists exception.
    /// </summary>
    [Serializable]
    public class UserNameAlreadyExistsException: Exception
    {
        /// <inheritdoc />
        public UserNameAlreadyExistsException() : base("Username already exists")
        {
        }

    }
}
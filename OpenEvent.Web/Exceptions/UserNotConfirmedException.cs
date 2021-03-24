using System;

namespace OpenEvent.Web.Exceptions
{
    /// <summary>
    /// User not confirmed exception
    /// </summary>
    [Serializable]
    public class UserNotConfirmedException : Exception
    {
        /// <inheritdoc />
        public UserNotConfirmedException() : base("User not confirmed")
        {
        }
    }
}
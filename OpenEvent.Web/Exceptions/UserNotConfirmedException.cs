using System;

namespace OpenEvent.Web.Exceptions
{
    [Serializable]
    public class UserNotConfirmedException : Exception
    {
        public UserNotConfirmedException() : base("User not confirmed")
        {
        }
    }
}
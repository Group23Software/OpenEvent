using System;

namespace OpenEvent.Web.Exceptions
{
    public class UserDoesNotOwnException : Exception
    {
        public UserDoesNotOwnException() : base("User does not own the event")
        {
        }
    }
}
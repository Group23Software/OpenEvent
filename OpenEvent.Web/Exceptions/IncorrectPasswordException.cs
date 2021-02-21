using System;

namespace OpenEvent.Web.Exceptions
{
    /// <summary>
    /// Incorrect Password Exception.
    /// </summary>
    [Serializable]
    public class IncorrectPasswordException : Exception
    {
        /// <inheritdoc />
        public IncorrectPasswordException() : base("Incorrect Password")
        {
        }
    }
}
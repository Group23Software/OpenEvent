using System;

namespace OpenEvent.Web.Exceptions
{
    /// <summary>
    /// Incorrect Password Exception.
    /// </summary>
    [Serializable]
    public class IncorrectPasswordException : Exception
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public IncorrectPasswordException() : base("Incorrect Password")
        {
        }
    }
}
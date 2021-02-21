using System;

namespace OpenEvent.Web.Exceptions
{
    /// <summary>
    /// Event not found exception.
    /// </summary>
    [Serializable]
    public class EventNotFoundException : Exception
    {
        /// <inheritdoc />
        public EventNotFoundException() : base("Event Not Found")
        {
        }
    }
}
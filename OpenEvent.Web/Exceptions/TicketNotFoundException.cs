using System;

namespace OpenEvent.Web.Exceptions
{
    /// <summary>
    /// Ticket not found exception.
    /// </summary>
    [Serializable]
    public class TicketNotFoundException : Exception
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public TicketNotFoundException() : base("Ticket Not Found")
        {
        }
    }
}
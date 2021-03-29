using System;

namespace OpenEvent.Data.Models.Analytic
{
    /// <summary>
    /// View model for ticket verification event
    /// </summary>
    public class TicketVerificationEventViewModel : AnalyticEvent
    {
        /// <inheritdoc />
        public override Guid Id { get; set; }

        /// <inheritdoc />
        public override DateTime Created { get; set; }

        /// <summary>
        /// Id of who owns the ticket
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Id of the ticket verified
        /// </summary>
        public Guid TicketId { get; set; }

        /// <summary>
        /// Id of the event the ticket is for
        /// </summary>
        public Guid EventId { get; set; }
    }
}
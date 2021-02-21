using System;

namespace OpenEvent.Web.Models.Analytic
{
    public class TicketVerificationEvent: AnalyticEvent
    {
        public override Guid Id { get; set; }
        public override DateTime Created { get; set; }
        // TODO : create members
    }
}
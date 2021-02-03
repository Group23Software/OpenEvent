using System;

namespace OpenEvent.Web.Models.Ticket
{
    /// <summary>
    /// Minimal ticket model.
    /// </summary>
    public class TicketViewModel
    {
        public Guid Id { get; set; }
        public string QRCode { get; set; }
    }
}
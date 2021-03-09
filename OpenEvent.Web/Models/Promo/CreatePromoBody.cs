using System;

namespace OpenEvent.Web.Models.Promo
{
    public class CreatePromoBody
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public bool Active { get; set; }
        public int Discount { get; set; }
        public Guid EventId { get; set; }
    }
}
using System;

namespace OpenEvent.Web.Models.Promo
{
    public class UpdatePromoBody
    {
        public Guid Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int Discount { get; set; }
        public bool Active { get; set; }
    }
}
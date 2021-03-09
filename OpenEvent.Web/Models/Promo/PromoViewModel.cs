using System;

namespace OpenEvent.Web.Models.Promo
{
    public class PromoViewModel
    {
        public Guid Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public bool Active { get; set; }
        public double Discount { get; set; }
    }
}
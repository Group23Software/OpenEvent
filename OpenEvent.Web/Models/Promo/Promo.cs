using System;

namespace OpenEvent.Web.Models.Promo
{
    public class Promo
    {
        public Guid Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public double Discount { get; set; }
        public bool Active { get; set; }
        public Event.Event Event { get; set; }
    }
}
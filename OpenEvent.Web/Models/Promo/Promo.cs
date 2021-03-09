using System;
using Newtonsoft.Json;

namespace OpenEvent.Web.Models.Promo
{
    public class Promo
    {
        public Guid Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int Discount { get; set; }
        public bool Active { get; set; }
        [JsonIgnore] public Event.Event Event { get; set; }
    }
}
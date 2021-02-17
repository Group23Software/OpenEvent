using System;
using System.ComponentModel.DataAnnotations;

namespace OpenEvent.Web.Models.PaymentMethod
{
    public class PaymentMethod
    {
        [Key]
        public string StripeCardId { get; set; }
        public User.User User { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Funding { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
        public string LastFour { get; set; }
        public string Country { get; set; }
    }
}
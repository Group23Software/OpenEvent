namespace OpenEvent.Web.Models.PaymentMethod
{
    public class PaymentMethodViewModel
    {
        public string StripeCardId { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Funding { get; set; }
        public long ExpiryMonth { get; set; }
        public long ExpiryYear { get; set; }
        public string LastFour { get; set; }
        public string Country { get; set; }
        public string NickName { get; set; }
        public bool IsDefault { get; set; }
    }
}
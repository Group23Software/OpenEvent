using Stripe;

namespace OpenEvent.Web.Models.User
{
    /// <summary>
    /// 
    /// </summary>
    public class StripeAccountInfo
    {
        /// <summary>
        /// Stripe account id
        /// </summary>
        public string PersonId { get; set; }

        /// <summary>
        /// If bank payouts are enabled for this account 
        /// </summary>
        public bool PayoutsEnabled { get; set; }

        /// <summary>
        /// If this account can accept payments
        /// </summary>
        public bool ChargesEnabled { get; set; }

        /// <summary>
        /// Default currency for this account (only gbp supported)
        /// </summary>
        public string DefaultCurrency { get; set; }

        /// <summary>
        /// Requirements needed to enable payouts and charges
        /// </summary>
        public AccountRequirements Requirements { get; set; }
    }
}
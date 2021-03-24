namespace OpenEvent.Web.Models.BankAccount
{
    /// <summary>
    /// Bank account view model
    /// </summary>
    public class BankAccountViewModel
    {
        /// <summary>
        /// Id of the bank account stored with Stripe
        /// </summary>
        public string StripeBankAccountId { get; set; }

        /// <summary>
        /// Name of the bank account
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Country of bank origin
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Bank's currency (only gbp supported)
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Last four digit of the bank account number
        /// </summary>
        public string LastFour { get; set; }

        /// <summary>
        /// Name of the bank eg: HSBC
        /// </summary>
        public string Bank { get; set; }
    }
}
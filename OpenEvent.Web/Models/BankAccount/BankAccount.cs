using System.ComponentModel.DataAnnotations;

namespace OpenEvent.Web.Models.BankAccount
{
    /// <summary>
    /// Full bank account model
    /// </summary>
    public class BankAccount
    {
        /// <summary>
        /// Id of the bank account stored with Stripe
        /// </summary>
        [Key]
        public string StripeBankAccountId { get; set; }

        /// <summary>
        /// The user who owns the bank account
        /// </summary>
        public User.User User { get; set; }

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
using System;

namespace OpenEvent.Web.Models.BankAccount
{
    /// <summary>
    /// Request body for adding a bank account 
    /// </summary>
    public class AddBankAccountBody
    {
        /// <summary>
        /// User's id
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Bank token returned by Stripe.js on the client side
        /// </summary>
        public string BankToken { get; set; }
    }
}
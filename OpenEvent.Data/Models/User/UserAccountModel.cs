using System;
using System.Collections.Generic;
using OpenEvent.Data.Models.PaymentMethod;
using OpenEvent.Data.Models.Transaction;

namespace OpenEvent.Data.Models.User
{
    /// <summary>
    /// Detailed user data for account page.
    /// </summary>
    public class UserAccountModel
    {
        /// <summary>
        /// User's id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Unique username
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// First name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Avatar bitmap image encoded as string
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// Unique phone number
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Date of birth
        /// </summary>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// If the user prefers dark mode
        /// </summary>
        public bool IsDarkMode { get; set; }

        /// <summary>
        /// User's connected Stripe account id
        /// </summary>
        public string StripeAccountId { get; set; }

        /// <summary>
        /// User's stripe customer id
        /// </summary>
        public string StripeCustomerId { get; set; }

        /// <summary>
        ///  User's address used for payment
        /// </summary>
        public Address.Address Address { get; set; }

        /// <summary>
        /// List of user's payment methods
        /// </summary>
        public List<PaymentMethodViewModel> PaymentMethods { get; set; }

        /// <summary>
        /// List of user's bank account
        /// </summary>
        public List<BankAccount.BankAccountViewModel> BankAccounts { get; set; }

        /// <summary>
        /// List of transactions associated with the user
        /// </summary>
        public List<TransactionViewModel> Transactions { get; set; }

        /// <summary>
        /// Account info gathered by the Stripe api
        /// </summary>
        public StripeAccountInfo StripeAccountInfo { get; set; }
    }
}
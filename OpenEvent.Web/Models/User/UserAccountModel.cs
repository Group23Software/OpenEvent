using System;
using System.Collections.Generic;
using OpenEvent.Web.Models.PaymentMethod;
using OpenEvent.Web.Models.Ticket;
using OpenEvent.Web.Models.Transaction;
using Stripe;

namespace OpenEvent.Web.Models.User
{
    /// <summary>
    /// Detailed user data for account page.
    /// </summary>
    public class UserAccountModel
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Avatar { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsDarkMode { get; set; }
        public string StripeAccountId { get; set; }
        public string StripeCustomerId { get; set; }
        public Address.Address Address { get; set; }
        public List<PaymentMethodViewModel> PaymentMethods { get; set; }
        public List<BankAccount.BankAccountViewModel> BankAccounts { get; set; }
        public List<TransactionViewModel> Transactions { get; set; }

        public StripeAccountInfo StripeAccountInfo { get; set; }
    }

    public class StripeAccountInfo
    {
        public string PersonId { get; set; }
        public bool PayoutsEnabled { get; set; }
        public bool ChargesEnabled { get; set; }
        public string DefaultCurrency { get; set; }

        public AccountRequirements Requirements { get; set; }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using OpenEvent.Web.Models.Analytic;
using OpenEvent.Web.Models.Recommendation;

namespace OpenEvent.Web.Models.User
{
    /// <summary>
    /// Base user model
    /// </summary>
    public class User
    {
        /// <summary>
        /// Unique id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Unique username
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Hashed password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// If the user has confirmed their email
        /// </summary>
        public bool Confirmed { get; set; }

        /// <summary>
        /// First name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Avatar bitmap image
        /// </summary>
        public byte[] Avatar { get; set; }

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
        /// List of all events the user hosts
        /// </summary>
        public List<Event.Event> HostedEvents { get; set; }

        /// <summary>
        /// List of tickets the user holds
        /// </summary>
        public List<Ticket.Ticket> Tickets { get; set; }

        /// <summary>
        /// User's connected Stripe account id
        /// </summary>
        public string StripeAccountId { get; set; }

        /// <summary>
        /// User's stripe customer id
        /// </summary>
        public string StripeCustomerId { get; set; }

        /// <summary>
        /// List of transactions associated with the user
        /// </summary>
        public List<Transaction.Transaction> Transactions { get; set; }

        /// <summary>
        /// List of user's payment methods
        /// </summary>
        public List<PaymentMethod.PaymentMethod> PaymentMethods { get; set; }

        /// <summary>
        /// List of user's bank account
        /// </summary>
        public List<BankAccount.BankAccount> BankAccounts { get; set; }

        /// <summary>
        /// User's address used for payment
        /// </summary>
        public Address.Address Address { get; set; }

        /// <summary>
        /// List of all user's page views
        /// </summary>
        public List<PageViewEvent> PageViewEvents { get; set; }

        /// <summary>
        /// List of all user's searches
        /// </summary>
        public List<SearchEvent> SearchEvents { get; set; }

        /// <summary>
        /// List of all user's ticket verifications
        /// </summary>
        public List<TicketVerificationEvent> VerificationEvents { get; set; }

        /// <summary>
        /// List of all user's recommendation scores
        /// </summary>
        public List<RecommendationScore> RecommendationScores { get; set; }

        /// <summary>
        /// Jwt token not saved in database 
        /// </summary>
        [NotMapped] public string Token { get; set; }
    }
}
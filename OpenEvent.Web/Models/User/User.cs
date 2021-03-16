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
    /// Full user model with all data.
    /// </summary>
    public class User
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public string Email { get; set; }

        public bool Confirmed { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public byte[] Avatar { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsDarkMode { get; set; }

        public List<Event.Event> HostedEvents { get; set; }

        public List<Ticket.Ticket> Tickets { get; set; }
        
        public string StripeAccountId { get; set; }
        
        public string StripeCustomerId { get; set; }
        
        public List<Transaction.Transaction> Transactions { get; set; }
        
        public List<PaymentMethod.PaymentMethod> PaymentMethods { get; set; }
        
        public List<BankAccount.BankAccount> BankAccounts { get; set; }
        
        public Address.Address Address { get; set; }
        
        public List<PageViewEvent> PageViewEvents { get; set; }
        public List<SearchEvent> SearchEvents { get; set; }
        public List<TicketVerificationEvent> VerificationEvents { get; set; }
        
        public List<RecommendationScore> RecommendationScores { get; set; }
        
        [NotMapped] public string Token { get; set; }
    }
}
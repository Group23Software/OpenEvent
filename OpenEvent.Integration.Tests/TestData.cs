using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using OpenEvent.Data.Models.Analytic;
using OpenEvent.Data.Models.Auth;
using OpenEvent.Data.Models.Category;
using OpenEvent.Data.Models.Event;
using OpenEvent.Data.Models.Promo;
using OpenEvent.Data.Models.Recommendation;
using OpenEvent.Data.Models.Ticket;
using OpenEvent.Data.Models.Transaction;
using OpenEvent.Data.Models.User;
using OpenEvent.Web;
using OpenEvent.Web.Contexts;
using Stripe;
using Address = OpenEvent.Data.Models.Address.Address;
using BankAccount = OpenEvent.Data.Models.BankAccount.BankAccount;
using Event = OpenEvent.Data.Models.Event.Event;
using PaymentMethod = OpenEvent.Data.Models.PaymentMethod.PaymentMethod;

namespace OpenEvent.Integration.Tests
{
    public static class TestData
    {
        public static string BaseUrl = "http://localhost:5000";

        public static async Task<UserViewModel> LogUserIn(HttpClient client)
        {
            LoginBody body = new()
            {
                Email = "exists@email.co.uk",
                Password = "Password",
                Remember = false
            };
            var response = await client.PostAsJsonAsync(new UriBuilder(BaseUrl + "/api/auth/login").Uri, body);
            var loggedUser = await response.Content.ReadFromJsonAsync<UserViewModel>();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loggedUser.Token);
            return loggedUser;

        }

        public static void InitializeDbForTests(ApplicationContext context, AppSettings appSettings)
        {
            StripeConfiguration.ApiKey = appSettings.StripeApiKey;
            
            PasswordHasher<User> hasher = new PasswordHasher<User>(
                new OptionsWrapper<PasswordHasherOptions>(
                    new PasswordHasherOptions
                    {
                        CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV2
                    })
            );

            context.AddRange(Categories);
            
            User.Password = hasher.HashPassword(User, "Password");
            
            var customerCreateOptions = new CustomerCreateOptions()
            {
                Email = User.Email,
                Phone = User.PhoneNumber,
                Name = User.FirstName + " " + User.LastName
            };
            var customerService = new CustomerService();
            var customer = customerService.Create(customerCreateOptions);
            User.StripeCustomerId = customer.Id;
            
            var accountCreateOptions = new AccountCreateOptions()
            {
                BusinessType = "individual",
                TosAcceptance = new AccountTosAcceptanceOptions() {Date = DateTime.Now, Ip = "127.0.0.1"},
                BusinessProfile = new AccountBusinessProfileOptions()
                    {Mcc = "7991", Url = "http://www.harrisonbarker.co.uk"},
                Individual = new AccountIndividualOptions()
                {
                    FirstName = User.FirstName,
                    LastName = User.LastName,
                    Dob = new DobOptions()
                    {
                        Year = User.DateOfBirth.Year,
                        Month = User.DateOfBirth.Month,
                        Day = User.DateOfBirth.Day
                    },
                    Address = new AddressOptions()
                    {
                        Line1 = User.Address.AddressLine1,
                        Line2 = User.Address.AddressLine2,
                        City = User.Address.City,
                        Country = User.Address.CountryCode,
                        PostalCode = User.Address.PostalCode
                    },
                    Email = User.Email,
                    Phone = User.PhoneNumber
                },
                Capabilities = new AccountCapabilitiesOptions
                {
                    CardPayments = new AccountCapabilitiesCardPaymentsOptions {Requested = true},
                    Transfers = new AccountCapabilitiesTransfersOptions {Requested = true},
                },
                Type = "custom"
            };

            var accountService = new AccountService();
            var account = accountService.Create(accountCreateOptions);
            User.StripeAccountId = account.Id;

            context.Add(User);
            context.Add(Event);
            context.SaveChanges();
        }

        public static readonly User User = new()
        {
            Id = new Guid("046E876E-D413-45AF-AC2A-552D7AA46C5C"),
            Email = "exists@email.co.uk",
            UserName = "ExistingUser",
            FirstName = "exists",
            LastName = "already",
            PhoneNumber = "0000000000",
            Avatar = new Byte[] {1, 1, 1, 1},
            IsDarkMode = false,
            StripeAccountId = "acct_1IPw2x2cYsy6zvuI",
            StripeCustomerId = "cus_J203DWUmiDnfRB",
            PaymentMethods = new List<PaymentMethod>
            {
                new()
                {
                    StripeCardId = "card_1IPw30K2ugLXrgQX4NDVSrfJ",
                    IsDefault = true
                }
            },
            BankAccounts = new List<BankAccount>
            {
                new()
                {
                    StripeBankAccountId = "ba_1IPycn2cYsy6zvuIWycwMVYN"
                }
            },
            Address = new Data.Models.Address.Address
            {
                AddressLine1 = "21 Wellsway",
                AddressLine2 = "",
                City = "Ipswich",
                CountryCode = "GB",
                CountryName = "United Kingdom",
                PostalCode = "IP14 6SL",
            },
            DateOfBirth = new DateTime(2000, 07, 24),
            Tickets = new List<Ticket>(),
            Transactions = new List<Transaction>(),
            HostedEvents = new List<Event>(),
            RecommendationScores = new List<RecommendationScore>(),
            SearchEvents = new List<SearchEvent>(),
            VerificationEvents = new List<TicketVerificationEvent>(),
            PageViewEvents = new List<PageViewEvent>(),
            Confirmed = true
        };

        public static readonly List<Category> Categories = new()
        {
            // , Id = new Guid("534DE110-2D1D-4AE8-9293-68FC8037DB5A")
            new() {Name = "Music", Id = new Guid("534DE110-2D1D-4AE8-9293-68FC8037DB5A")},
            new() {Name = "Business"},
            new() {Name = "Charity"},
            new() {Name = "Culture"},
            new() {Name = "Family"},
            new() {Name = "Education"},
            new() {Name = "Fashion"},
            new() {Name = "Film"},
            new() {Name = "Media"},
            new() {Name = "Food"},
            new() {Name = "Politics"},
            new() {Name = "Health"},
            new() {Name = "Hobbies"},
            new() {Name = "Lifestyle"},
            new() {Name = "Other"},
            new() {Name = "Performing"},
            new() {Name = "Visual Arts"},
            new() {Name = "Religion"},
            new() {Name = "Science"},
            new() {Name = "Technology"},
            new() {Name = "Seasonal"},
            new() {Name = "Sport"},
            new() {Name = "Outdoor"},
            new() {Name = "Travel"},
            new() {Name = "Automobile"}
        };

        public static Event Event = new()
        {
            Id = new Guid("74831876-FC2E-4D03-99D8-B3872BDEFD5C"),
            Address = new Address
            {
                AddressLine1 = "Main Street",
                AddressLine2 = "",
                City = "City",
                CountryCode = "GB",
                CountryName = "United Kingdom",
                PostalCode = "AA1 1AA",
                Lat = 51.47338,
                Lon = -0.08375
            },
            Description = "This is a test event",
            Images = new List<Image>
            {
                new() {Label = "Image", Source = new Byte[] {1, 1, 1, 1}}
            },
            isCanceled = false,
            Name = "Test Event",
            Host = User,
            Price = 1010,
            Thumbnail = new Image {Label = "Thumbnail", Source = new Byte[] {1, 1, 1, 1}},
            EndLocal = new DateTime(),
            EndUTC = new DateTime(),
            StartLocal = new DateTime(),
            StartUTC = new DateTime(),
            IsOnline = false,
            SocialLinks = new List<SocialLink> {new() {Link = "custom.co.uk", SocialMedia = SocialMedia.Site}},
            Tickets = new List<Ticket>
            {
                new()
                {
                    Id = new Guid("A85DDDF9-C5ED-469C-914F-75097B950024"),
                    Available = false,
                    User = User,
                    Uses = 0,
                    QRCode = new Byte[] {0, 0, 0, 0},
                    Transaction = new Transaction
                    {
                        StripeIntentId = "1",
                        Amount = 100,
                        End = DateTime.Now,
                        Start = DateTime.Now,
                        Updated = DateTime.Now,
                        Paid = true,
                        Status = PaymentStatus.succeeded,
                        User = User
                    }
                },
                new()
                {
                    Available = true,
                    Uses = 0
                }
            },
            Transactions = new List<Transaction>(),
            VerificationEvents = new List<TicketVerificationEvent>(),
            PageViewEvents = new List<PageViewEvent>(),
            EventCategories = new List<EventCategory>(),
            Promos = new List<Promo>
            {
                new()
                {
                    Id = new Guid("AB261AEC-B56A-4D12-A9CC-8F499B98D4B1"),
                    Active = true,
                    Discount = 50,
                    Start = DateTime.Now,
                    End = DateTime.Now.AddMonths(1)
                }
            }
        };
    }
}
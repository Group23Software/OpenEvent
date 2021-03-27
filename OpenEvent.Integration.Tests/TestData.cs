using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using OpenEvent.Web.Contexts;
using OpenEvent.Web.Models.Address;
using OpenEvent.Web.Models.Analytic;
using OpenEvent.Web.Models.Auth;
using OpenEvent.Web.Models.BankAccount;
using OpenEvent.Web.Models.Category;
using OpenEvent.Web.Models.Event;
using OpenEvent.Web.Models.PaymentMethod;
using OpenEvent.Web.Models.Promo;
using OpenEvent.Web.Models.Recommendation;
using OpenEvent.Web.Models.Ticket;
using OpenEvent.Web.Models.Transaction;
using OpenEvent.Web.Models.User;

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
            return await response.Content.ReadFromJsonAsync<UserViewModel>();
        }

        public static void InitializeDbForTests(ApplicationContext context)
        {
            PasswordHasher<User> hasher = new PasswordHasher<User>(
                new OptionsWrapper<PasswordHasherOptions>(
                    new PasswordHasherOptions
                    {
                        CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV2
                    })
            );

            context.AddRange(Categories);
            User.Password = hasher.HashPassword(User, "Password");
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
            Address = new Address
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
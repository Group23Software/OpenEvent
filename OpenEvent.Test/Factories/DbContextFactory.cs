using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Castle.Components.DictionaryAdapter;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OpenEvent.Data.Models.Address;
using OpenEvent.Data.Models.Analytic;
using OpenEvent.Data.Models.BankAccount;
using OpenEvent.Data.Models.Category;
using OpenEvent.Data.Models.Event;
using OpenEvent.Data.Models.PaymentMethod;
using OpenEvent.Data.Models.Promo;
using OpenEvent.Data.Models.Recommendation;
using OpenEvent.Data.Models.Ticket;
using OpenEvent.Data.Models.Transaction;
using OpenEvent.Data.Models.User;
using OpenEvent.Test.Setups;
using OpenEvent.Web.Contexts;

namespace OpenEvent.Test.Factories
{
    public class DbContextFactory : IDisposable
    {
        private DbConnection _connection;

        private DbContextOptions<ApplicationContext> CreateOptions()
        {
            return new DbContextOptionsBuilder<ApplicationContext>()
                .EnableSensitiveDataLogging()
                .UseSqlite(_connection).Options;
        }

        public ApplicationContext CreateContext()
        {
            if (_connection == null)
            {
                _connection = new SqliteConnection("DataSource=:memory:;Foreign Keys=False");
                _connection.Open();

                var options = CreateOptions();
                using (var context = new ApplicationContext(options))
                {
                    context.Database.EnsureDeleted();
                    context.Database.EnsureCreated();
                    Seed(context);
                }
            }

            return new ApplicationContext(CreateOptions());
        }

        public void Dispose()
        {
            if (_connection != null)
            {
                _connection.Dispose();
                _connection = null;
            }
        }

        public void Seed(ApplicationContext context)
        {
            PasswordHasher<User> hasher = new PasswordHasher<User>(
                new OptionsWrapper<PasswordHasherOptions>(
                    new PasswordHasherOptions
                    {
                        CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV2
                    })
            );

            List<Category> categories = new List<Category>
            {
                // , Id = new Guid("534DE110-2D1D-4AE8-9293-68FC8037DB5A")
                new() {Name = "Music"},
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

            List<User> users = new List<User>
            {
                new()
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
                    BankAccounts = new EditableList<BankAccount>
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
                }
            };

            users[0].Password = hasher.HashPassword(users[0], "Password");

            List<Event> events = new List<Event>
            {
                new()
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
                            User = users[0],
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
                                User = users[0]
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
                },
                new()
                {
                    Id = new Guid("4D133B74-C2B3-445B-8C55-F8D785C8CB4A"),
                    Description = "This is a online event",
                    Images = new List<Image>
                    {
                        new() {Label = "Image", Source = new Byte[] {1, 1, 1, 1}}
                    },
                    isCanceled = false,
                    Name = "Online Event",
                    Price = 2222,
                    Thumbnail = new Image {Label = "Thumbnail", Source = new Byte[] {1, 1, 1, 1}},
                    EndLocal = new DateTime(),
                    EndUTC = new DateTime(),
                    StartLocal = new DateTime(),
                    StartUTC = new DateTime(),
                    IsOnline = true,
                    SocialLinks = new List<SocialLink> {new() {Link = "custom.co.uk", SocialMedia = SocialMedia.Site}},
                    Tickets = new List<Ticket>
                    {
                        new()
                        {
                            Id = new Guid("F8968DA6-2CBD-4DD0-8CFA-4DCE26D326EB"),
                            Available = false,
                            User = users[0],
                            Uses = 0,
                            QRCode = new Byte[] {0, 0, 0, 0},
                            Transaction = new Transaction
                            {
                                StripeIntentId = "2",
                                Amount = 200,
                                End = DateTime.Now,
                                Start = DateTime.Now,
                                Updated = DateTime.Now,
                                Paid = true,
                                Status = PaymentStatus.succeeded,
                                User = users[0]
                            }
                        }
                    },
                    Transactions = new List<Transaction>(),
                    EventCategories = new List<EventCategory>(),
                    VerificationEvents = new List<TicketVerificationEvent>(),
                    PageViewEvents = new List<PageViewEvent>(),
                },
                new()
                {
                    Id = new Guid("70FB9650-A834-464A-B736-B2C4A211A790"),
                    Description = "This is a online event with categories",
                    Images = new List<Image>
                    {
                        new() {Label = "Image", Source = new Byte[] {1, 1, 1, 1}}
                    },
                    isCanceled = false,
                    Name = "Online Event with categories",
                    Price = 2222,
                    Thumbnail = new Image {Label = "Thumbnail", Source = new Byte[] {1, 1, 1, 1}},
                    EndLocal = new DateTime(),
                    EndUTC = new DateTime(),
                    StartLocal = new DateTime(),
                    StartUTC = new DateTime(),
                    IsOnline = true,
                    SocialLinks = new List<SocialLink> {new() {Link = "custom.co.uk", SocialMedia = SocialMedia.Site}},
                    Tickets = new List<Ticket>
                    {
                        new()
                        {
                            Id = new Guid("8EAB61F7-84EA-4F8E-878A-AA3FEF72FD8F"),
                            Available = false,
                            User = users[0],
                            Uses = 0,
                            QRCode = new Byte[] {0, 0, 0, 0},
                            Transaction = new Transaction
                            {
                                StripeIntentId = "3",
                                Amount = 200,
                                End = DateTime.Now,
                                Start = DateTime.Now,
                                Updated = DateTime.Now,
                                Paid = true,
                                Status = PaymentStatus.succeeded,
                                User = users[0]
                            }
                        }
                    },
                    Transactions = new List<Transaction>(),
                    EventCategories = new List<EventCategory>(),
                    VerificationEvents = new List<TicketVerificationEvent>(),
                    PageViewEvents = new List<PageViewEvent>(),
                }
            };


            // ********** Relationships ********** 

            // events[0].Host = users[0];
            events.ForEach(x => x.Host = users[0]);

            List<PageViewEvent> pageViewEvents = new List<PageViewEvent>
            {
                new()
                {
                    Created = DateTime.Now,
                    Event = events[0],
                    User = users[0]
                }
            };

            for (int i = 0; i < 10; i++)
            {
                pageViewEvents.Add(new PageViewEvent
                    {Created = new DateTime(DateTime.Now.Ticks).AddHours(i), Event = events[0], User = users[0]});
            }

            List<TicketVerificationEvent> ticketVerificationEvents = new List<TicketVerificationEvent>
            {
                new()
                {
                    Created = DateTime.Now,
                    Event = events[0],
                    Ticket = events[0].Tickets[0],
                    User = users[0]
                }
            };

            List<SearchEvent> searchEvents = new List<SearchEvent>
            {
                new()
                {
                    Created = DateTime.Now,
                    User = users[0],
                    Params = "Events",
                    Search = "Category:Id"
                }
            };

            for (int i = 0; i < 10; i++)
            {
                searchEvents.Add(new SearchEvent
                {
                    Created = new DateTime(DateTime.Now.Ticks).AddHours(i), Params = $"Category{Guid.NewGuid()}",
                    Search = "", User = users[0]
                });
            }

            // ********** ************* ********** 

            context.Users.AddRange(users);
            context.Categories.AddRange(categories);
            context.Events.AddRange(events);
            context.PageViewEvents.AddRange(pageViewEvents);
            context.VerificationEvents.AddRange(ticketVerificationEvents);
            context.SearchEvents.AddRange(searchEvents);
            context.SaveChanges();

            // ******** <-> Relationships ********

            context.Events.First().EventCategories.Add(new EventCategory {CategoryId = context.Categories.First().Id});
            context.Users.First().RecommendationScores = context.Categories.ToList()
                .Select(x => new RecommendationScore {Category = x, Weight = 0}).ToList();
            context.SaveChanges();
        }
    }
}
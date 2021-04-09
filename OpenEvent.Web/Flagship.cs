using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenEvent.Data;
using OpenEvent.Data.Models.Address;
using OpenEvent.Data.Models.Analytic;
using OpenEvent.Data.Models.BankAccount;
using OpenEvent.Data.Models.Category;
using OpenEvent.Data.Models.Event;
using OpenEvent.Data.Models.PaymentMethod;
using OpenEvent.Data.Models.Promo;
using OpenEvent.Data.Models.Recommendation;
using OpenEvent.Data.Models.Ticket;
using OpenEvent.Data.Models.User;
using OpenEvent.Web.Contexts;
using OpenEvent.Web.Services;

namespace OpenEvent.Web
{
    public static class Flagship
    {
        private const int NumberOfUsers = 100;
        private const int NumberOfEvents = 50;
        private const int NumberOfPagViews = 10;
        private const int NumberOfSearches = 2;

        public static async Task SeedCategories(ApplicationContext context)
        {
            List<Category> categories = new List<Category>
            {
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

            context.Categories.AddRange(categories);
            await context.SaveChangesAsync();
        }

        public static async Task SeedDatabase(ApplicationContext context, IServiceProvider serviceProvider)
        {
            await SeedUsers(context, serviceProvider);
            await SeedEvents(context, serviceProvider);
            await SeedInteraction(context);
        }

        private static async Task SeedUsers(ApplicationContext context, IServiceProvider serviceProvider)
        {
            var categories = await context.Categories.ToListAsync();

            PasswordHasher<User> hasher = new PasswordHasher<User>(
                new OptionsWrapper<PasswordHasherOptions>(
                    new PasswordHasherOptions()
                    {
                        CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV2
                    })
            );

            var users = UserData.FakeUser.Generate(NumberOfUsers);

            users.ForEach(u =>
            {
                u.RecommendationScores =
                    categories.Select(x => new RecommendationScore {Category = x, Weight = 0}).ToList();
                u.Password = hasher.HashPassword(u, "Password");
            });

            var testUser = new User
            {
                Id = new Guid("23C6C78B-5754-48A5-82E3-5772AA462CDE"),
                Email = "test@test.co.uk",
                FirstName = "Test",
                LastName = "User",
                UserName = "TestUser",
                Avatar = Encoding.UTF8.GetBytes("https://picsum.photos/640/480/?image=41"),
                Address = new Address
                {
                    City = "Stowmarket",
                    AddressLine1 = "21 Wellsway",
                    PostalCode = "IP146SL",
                    CountryCode = "GB",
                    CountryName = "United Kingdom"
                },
                PhoneNumber = "+4407852276048",
                IsDarkMode = false,
                Confirmed = true,
                DateOfBirth = new DateTime(2000,7,24),  
                StripeAccountId = "acct_1IeMUv2euIlqIANk",
                StripeCustomerId = "cus_JGuDCovqvDgFjb",
                PaymentMethods = new List<PaymentMethod>()
                {
                    new()
                    {
                        StripeCardId = "card_1IeMQzK2ugLXrgQX48FGjxMT",
                        LastFour = "0005",
                        Brand = "Visa",
                        Funding = "debit",
                        ExpiryMonth = 11,
                        ExpiryYear = 2024,
                        Country = "GB",
                        NickName = "Debit",
                        Name = "TestUser",
                        IsDefault = true
                    }
                },
                BankAccounts = new List<BankAccount>()
                {
                    new()
                    {
                        Bank = "STRIPE TEST BANK",
                        StripeBankAccountId = "ba_1IeMXB2euIlqIANkrYY0NuFS",
                        Currency = "gbp",
                        Country = "GB",
                        LastFour = "2345",
                        Name = "Test User"
                    }
                },
                RecommendationScores = categories.Select(x => new RecommendationScore {Category = x, Weight = 0}).ToList()
            };
            testUser.Password = hasher.HashPassword(testUser, "Password@1");

            await context.Users.AddAsync(testUser);
            await context.Users.AddRangeAsync(users);
            await context.SaveChangesAsync();
        }

        private static async Task SeedEvents(ApplicationContext context, IServiceProvider serviceProvider)
        {
            var categories = await context.Categories.ToListAsync();
            var users = await context.Users.ToListAsync();
            var testUser = await context.Users.FirstOrDefaultAsync(x => x.Id == new Guid("23C6C78B-5754-48A5-82E3-5772AA462CDE"));

            var events = EventData.FakeEvent.Generate(NumberOfEvents);
            var testUsersEvents = EventData.FakeEvent.Generate(3);
            
            var tickets = new List<Ticket>();

            events.ForEach(GenerateEvent(users, categories, tickets));
            testUsersEvents.ForEach(GenerateEvent(new List<User> {testUser},categories,tickets));
            
            
            await context.Events.AddRangeAsync(events);
            await context.Tickets.AddRangeAsync(tickets);
            await context.SaveChangesAsync();
        }

        private static Action<Event> GenerateEvent(List<User> users, List<Category> categories, List<Ticket> tickets)
        {
            return e =>
            {
                if (new Random().Next() > int.MaxValue / 2)
                {
                    e.Promos = new List<Promo>()
                    {
                        GeneratePromo(e)
                    };
                }

                e.Host = users.ElementAt(new Random().Next(users.Count));

                var randCategories = new List<Category>();
                while (randCategories.Count < 4)
                {
                    var c = categories.ElementAt(new Random().Next(categories.Count));
                    if (!randCategories.Contains(c)) randCategories.Add(c);
                }

                e.EventCategories = new List<EventCategory>();
                randCategories.ForEach(c =>
                {
                    e.EventCategories.Add(new EventCategory()
                    {
                        Category = c
                    });
                });

                e.Tickets = new List<Ticket>();
                var numberOfTickets = new Random().Next(1000);
                e.TicketsLeft = numberOfTickets;
                for (int i = 0; i < numberOfTickets; i++)
                {
                    tickets.Add(new Ticket()
                    {
                        Available = true,
                        Uses = 0,
                        Event = e
                    });
                }
            };
        }

        private static async Task SeedInteraction(ApplicationContext context)
        {
            var users = await context.Users.ToListAsync();
            var events = await context.Events.ToListAsync();
            var categories = await context.Categories.ToListAsync();

            var pageViews = new List<PageViewEvent>();
            var searches = new List<SearchEvent>();

            users.ForEach(u =>
            {
                for (int i = 0; i < NumberOfPagViews; i++)
                {
                    var e = events.ElementAt(new Random().Next(events.Count));
                    pageViews.Add(new PageViewEvent
                    {
                        Created = RandomDate(e.Created, DateTime.Now),
                        Event = e,
                        User = u
                    });
                }

                for (int i = 0; i < NumberOfSearches; i++)
                {
                    SearchFilter filter = new SearchFilter()
                    {
                        Key = SearchParam.Category,
                        Value = categories.ElementAt(new Random().Next(categories.Count)).Id.ToString()
                    };

                    searches.Add(new SearchEvent()
                    {
                        Created = RandomDate(DateTime.Now.AddMonths(-3), DateTime.Now),
                        Params = String.Join(",", filter),
                        Search = "",
                        User = u
                    });
                }
            });

            await context.PageViewEvents.AddRangeAsync(pageViews);
            await context.SearchEvents.AddRangeAsync(searches);
            await context.SaveChangesAsync();
        }

        private static DateTime RandomDate(DateTime start, DateTime end)
        {
            var startSpan = end - start;
            var startNewSpan = new TimeSpan(0, new Random().Next(0, (int) startSpan.TotalMinutes), 0);
            return start + startNewSpan;
        }

        private static Promo GeneratePromo(Event e)
        {
            var start = RandomDate(e.Created, e.EndLocal);
            var end = RandomDate(start, e.EndLocal);

            return new Promo
            {
                Active = true,
                Discount = new Random().Next(10, 50),
                Start = start,
                End = end
            };
        }
    }
}
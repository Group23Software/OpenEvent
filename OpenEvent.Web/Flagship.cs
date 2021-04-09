using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenEvent.Data;
using OpenEvent.Data.Models.Analytic;
using OpenEvent.Data.Models.Category;
using OpenEvent.Data.Models.Event;
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

            await context.Users.AddRangeAsync(users);
            await context.SaveChangesAsync();
        }

        private static async Task SeedEvents(ApplicationContext context, IServiceProvider serviceProvider)
        {
            var categories = await context.Categories.ToListAsync();
            var users = await context.Users.ToListAsync();

            // var eventService = serviceProvider.GetRequiredService<IEventService>();
            //
            // var newEvents = EventData.FakeCreateEvent.Generate(10);
            //
            // foreach (var createEventBody in newEvents)
            // {
            //     createEventBody.HostId = users.ElementAt(new Random().Next(users.Count)).Id;
            //     createEventBody.Categories = new List<Category> {categories.ElementAt(new Random().Next(categories.Count))};
            //     await eventService.Create(createEventBody);
            // }

            var events = EventData.FakeEvent.Generate(NumberOfEvents);
            var tickets = new List<Ticket>();

            events.ForEach(e =>
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
            });


            await context.Events.AddRangeAsync(events);
            await context.Tickets.AddRangeAsync(tickets);
            await context.SaveChangesAsync();
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
                        Created = RandomDate(DateTime.Now.AddMonths(-3),DateTime.Now),
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.Components.DictionaryAdapter;
using Microsoft.EntityFrameworkCore;
using OpenEvent.Web.Contexts;
using OpenEvent.Web.Models.User;
using EntityFrameworkCoreMock;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using OpenEvent.Web.Models.Address;
using OpenEvent.Web.Models.Analytic;
using OpenEvent.Web.Models.BankAccount;
using OpenEvent.Web.Models.Category;
using OpenEvent.Web.Models.Event;
using OpenEvent.Web.Models.PaymentMethod;
using OpenEvent.Web.Models.Ticket;

namespace OpenEvent.Test.Setups
{
    public class BasicSetup
    {
        public async Task<DbContextMock<ApplicationContext>> Setup()
        {
            var dbContextOptions = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: "openEvent").Options;

            PasswordHasher<User> hasher = new PasswordHasher<User>(
                new OptionsWrapper<PasswordHasherOptions>(
                    new PasswordHasherOptions()
                    {
                        CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV2
                    })
            );


            List<User> seedUserList = new List<User>
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
                    PaymentMethods = new List<PaymentMethod>()
                    {
                        new()
                        {
                            StripeCardId = "card_1IPw30K2ugLXrgQX4NDVSrfJ",
                            IsDefault = true
                        }
                    },
                    BankAccounts = new EditableList<BankAccount>()
                    {
                        new()
                        {
                            StripeBankAccountId = "ba_1IPycn2cYsy6zvuIWycwMVYN"
                        }
                    },
                    Address = new Address()
                    {
                        AddressLine1 = "21 Wellsway",
                        AddressLine2 = "",
                        City = "Ipswich",
                        CountryCode = "GB",
                        CountryName = "United Kingdom",
                        PostalCode = "IP14 6SL",
                    },
                    DateOfBirth = new DateTime(2000, 07, 24),
                    Tickets = new List<Ticket>()
                    {
                        new()
                        {
                            Id = Guid.NewGuid(),
                            QRCode = new Byte[] {0, 0, 0, 0},
                        },
                        new()
                        {
                            Id = Guid.NewGuid(),
                            QRCode = new Byte[] {0, 0, 0, 0},
                        }
                    }
                }
            };

            IQueryable<Category> seedCategories = new List<Category>
            {
                new()
                {
                    Id = new Guid("534DE110-2D1D-4AE8-9293-68FC8037DB5A"),
                    Name = "Music"
                },
                new()
                {
                    Id = new Guid("08CC5B09-70E2-4215-9B35-1E6A067A0204"),
                    Name = "Comedy"
                }
            }.AsQueryable();

            List<Event> seedEvents = new List<Event>
            {
                new()
                {
                    Id = new Guid("74831876-FC2E-4D03-99D8-B3872BDEFD5C"),
                    Address = new Address()
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
                    Host = seedUserList[0],
                    Images = new List<Image>()
                    {
                        new() {Label = "Image", Source = new Byte[] {1, 1, 1, 1}}
                    },
                    isCanceled = false,
                    Name = "Test Event",
                    Price = 1010,
                    Thumbnail = new Image() {Label = "Thumbnail", Source = new Byte[] {1, 1, 1, 1}},
                    EndLocal = new DateTime(),
                    EndUTC = new DateTime(),
                    StartLocal = new DateTime(),
                    StartUTC = new DateTime(),
                    IsOnline = false,
                    SocialLinks = new List<SocialLink> {new() {Link = "custom.co.uk", SocialMedia = SocialMedia.Site}},
                    Tickets = new List<Ticket>()
                    {
                        new()
                        {
                            Id = new Guid("892C6AE2-0F9A-4125-9E95-FAC401A4EF60"),
                            User = seedUserList[0],
                            QRCode = new Byte[] {0, 0, 0, 0}
                        },
                        new()
                        {
                            Id = new Guid("420BF325-27C1-43F7-BC4A-80F459D67356"),
                            QRCode = new Byte[] {0, 0, 0, 0}
                        }
                    },
                    EventCategories = new List<EventCategory>
                    {
                        new()
                        {
                            CategoryId = new Guid("534DE110-2D1D-4AE8-9293-68FC8037DB5A"),
                            EventId = new Guid("74831876-FC2E-4D03-99D8-B3872BDEFD5C")
                        }
                    },
                    PageViewEvents = new List<PageViewEvent>()
                },
                new()
                {
                    Id = new Guid("5F35AA8F-4CC5-4E1A-AB73-6875D5769715"),
                    Description = "This is a different test event",
                    Host = new User() {Id = new Guid("046E876E-D413-45AF-AC2A-552D7AA46C5C")},
                    Images = new List<Image>()
                    {
                        new() {Label = "Image", Source = new Byte[] {1, 1, 1, 1}}
                    },
                    isCanceled = false,
                    Name = "Test different Event",
                    Price = 1010,
                    Thumbnail = new Image() {Label = "Thumbnail", Source = new Byte[] {1, 1, 1, 1}},
                    EndLocal = new DateTime(),
                    EndUTC = new DateTime(),
                    StartLocal = new DateTime(),
                    StartUTC = new DateTime(),
                    IsOnline = true,
                    SocialLinks = new List<SocialLink> {new() {Link = "custom.co.uk", SocialMedia = SocialMedia.Site}},
                    Tickets = new List<Ticket>(),
                    EventCategories = new List<EventCategory>
                    {
                        new()
                        {
                            CategoryId = new Guid("08CC5B09-70E2-4215-9B35-1E6A067A0204"),
                            EventId = new Guid("5F35AA8F-4CC5-4E1A-AB73-6875D5769715")
                        }
                    },
                    PageViewEvents = new List<PageViewEvent>()
                }
            };

            IQueryable<Ticket> seedTickets = new List<Ticket>()
            {
                new()
                {
                    Id = new Guid("853F592D-D454-4FA1-BC9B-12991C13D835"),
                    User = seedUserList[0],
                    QRCode = new Byte[] {0, 0, 0, 0},
                    Event = seedEvents[0]
                }
            }.AsQueryable();

            seedUserList[0].Password = hasher.HashPassword(seedUserList[0], "Password");


            IQueryable<User> seedUsers = seedUserList.AsQueryable();


            var mockContext = new DbContextMock<ApplicationContext>(dbContextOptions);
            var userDbSetMock = mockContext.CreateDbSetMock(x => x.Users, seedUsers);
            var eventDbSetMock = mockContext.CreateDbSetMock(x => x.Events, seedEvents.AsQueryable());
            var categoryDbSetMock = mockContext.CreateDbSetMock(x => x.Categories, seedCategories);
            var ticketDbSetMock = mockContext.CreateDbSetMock(x => x.Tickets, seedTickets);

            return mockContext;
        }
    }
}
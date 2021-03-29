using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using OpenEvent.Data.Models.Address;
using OpenEvent.Data.Models.Category;
using OpenEvent.Data.Models.Event;
using OpenEvent.Test.Factories;
using OpenEvent.Test.Setups;
using OpenEvent.Web.Exceptions;

namespace OpenEvent.Test.Services.EventService
{
    public class Create
    {
        private readonly CreateEventBody NoHostCreateBody = new()
        {
            Name = "Test create event",
            Description = "this is a test create event",
            Address = new Address() { },
            Categories = new List<Category>(),
            Images = new List<ImageViewModel>(),
            Price = 1010,
            Thumbnail = new ImageViewModel(),
            EndLocal = new DateTime(),
            HostId = new Guid(),
            IsOnline = false,
            SocialLinks = new List<SocialLinkViewModel>(),
            StartLocal = new DateTime(),
            NumberOfTickets = 100
        };

        [Test]
        public async Task Should_Create_New_Event()
        {
            await using (var context = new DbContextFactory().CreateContext())
            {
                var service = new EventServiceFactory().Create(context);

                var validCreateEventBody = Data.Data.FakeCreateEventBody.Generate();
                validCreateEventBody.Categories = await context.Categories.ToListAsync();
                var result = await service.Create(validCreateEventBody);
                result.Should().NotBeNull();

                var check = context.Events.FirstOrDefault(x => x.Name == validCreateEventBody.Name);
                check.Should().NotBeNull();
            }
        }

        [Test]
        public async Task ShouldCreateEmptyTickets()
        {
            await using (var context = new DbContextFactory().CreateContext())
            {
                var service = new EventServiceFactory().Create(context);

                var validCreateEventBody = Data.Data.FakeCreateEventBody.Generate();
                validCreateEventBody.Categories = await context.Categories.ToListAsync();
                var result = await service.Create(validCreateEventBody);
                result.Should().NotBeNull();

                var tickets = await context.Tickets.Where(x => x.Event.Name == validCreateEventBody.Name)
                    .ToListAsync();
                tickets.Count().Should().Be(validCreateEventBody.NumberOfTickets);
            }
        }

        // [Test]
        // public async Task Should_Not_Find_Address()
        // {
        //     // TODO   
        // }

        [Test]
        public async Task ShouldNotFindHost()
        {
            await using (var context = new DbContextFactory().CreateContext())
            {
                var service = new EventServiceFactory().Create(context);
                FluentActions.Invoking(async () => await service.Create(NoHostCreateBody))
                    .Should().Throw<UserNotFoundException>();
            }
        }

        // [Test]
        // [Ignore("Need to make separate mock")]
        // public async Task ShouldThrowDbUpdateException()
        // {
        //     MockContext.Setup(c => c.SaveChangesAsync(new CancellationToken()))
        //         .ReturnsAsync(() => throw new DbUpdateException());
        //
        //     FluentActions.Invoking(async () => await EventService.Create(ValidCreateEventBody)).Should()
        //         .Throw<DbUpdateException>();
        // }

        [Test]
        public async Task Event_Should_Have_Host()
        {
            using (var context = new DbContextFactory().CreateContext())
            {
                var service = new EventServiceFactory().Create(context);
                var validCreateEventBody = Data.Data.FakeCreateEventBody.Generate();
                validCreateEventBody.Categories = await context.Categories.ToListAsync();
                var result = await service.Create(validCreateEventBody);
                result.Should().NotBeNull();

                var check = await context.Events.Include(x => x.Host).FirstOrDefaultAsync(x => x.Name == validCreateEventBody.Name);
                check.Host.Id.Should().Be(new Guid("046E876E-D413-45AF-AC2A-552D7AA46C5C"));
            }
        }
    }
}
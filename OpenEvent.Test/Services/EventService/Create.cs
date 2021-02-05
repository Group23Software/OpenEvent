using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using OpenEvent.Web.Exceptions;
using OpenEvent.Web.Models.Category;
using OpenEvent.Web.Models.Event;

namespace OpenEvent.Test.Services.EventService
{
    [TestFixture]
    public class Create : EventTestFixture
    {
        
        private readonly CreateEventBody ValidCreateEventBody = new CreateEventBody()
        {
            Name = "Test create event",
            Description = "this is a test create event",
            Address = new Address(){},
            Categories = new List<Category>(),
            Images = new List<ImageViewModel>(),
            Price = new decimal(10.10),
            Thumbnail = new ImageViewModel(),
            EndLocal = new DateTime(),
            HostId = new Guid("046E876E-D413-45AF-AC2A-552D7AA46C5C"),
            IsOnline = false,
            SocialLinks = new List<SocialLinkViewModel>(),
            StartLocal = new DateTime(),
            NumberOfTickets = 100
        };
        
        private readonly CreateEventBody NoHostCreateBody = new CreateEventBody()
        {
            Name = "Test create event",
            Description = "this is a test create event",
            Address = new Address(){},
            Categories = new List<Category>(),
            Images = new List<ImageViewModel>(),
            Price = new decimal(10.10),
            Thumbnail = new ImageViewModel(),
            EndLocal = new DateTime(),
            HostId = new Guid(),
            IsOnline = false,
            SocialLinks = new List<SocialLinkViewModel>(),
            StartLocal = new DateTime(),
            NumberOfTickets = 100
        };
        
        [Test]
        public async Task ShouldCreateNewEvent()
        {
            var result = await EventService.Create(ValidCreateEventBody);
            result.Should().NotBeNull();

            var check = MockContext.Object.Events.FirstOrDefault(x => x.Name == "Test create event");
            check.Should().NotBeNull();
        }
        
        [Test]
        public async Task ShouldCreateEmptyTickets()
        {
            var result = await EventService.Create(ValidCreateEventBody);
            result.Should().NotBeNull();

            var check = MockContext.Object.Events.FirstOrDefault(x => x.Name == "Test create event");
            check.Should().NotBeNull();
            check.Tickets.Count.Should().Be(ValidCreateEventBody.NumberOfTickets);
            check.TicketsLeft.Should().Be(ValidCreateEventBody.NumberOfTickets);
        }
        
        [Test]
        public async Task ShouldNotFindHost()
        {
            FluentActions.Invoking(async () => await EventService.Create(NoHostCreateBody))
                .Should().Throw<UserNotFoundException>();
        }

        [Test]
        public async Task ShouldThrowDbUpdateException()
        {
            MockContext.Setup(c => c.SaveChangesAsync(new CancellationToken()))
                .ReturnsAsync(() => throw new DbUpdateException());
            
            FluentActions.Invoking(async () => await EventService.Create(ValidCreateEventBody)).Should().Throw<DbUpdateException>();
        }
    }
}
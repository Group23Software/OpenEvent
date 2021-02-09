using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using OpenEvent.Web.Exceptions;
using OpenEvent.Web.Models.Event;

namespace OpenEvent.Test.Controllers.EventController
{
    [TestFixture]
    public class Update
    {
        private readonly Mock<Web.Services.IEventService> EventServiceMock = new();

        private readonly UpdateEventBody UpdateEventBody = new()
        {
            Id = new Guid("74831876-FC2E-4D03-99D8-B3872BDEFD5C"),
            Name = "Updated name",
            Address = new Address()
            {
                AddressLine1 = "Updated Main Street",
                AddressLine2 = "",
                City = "City of update",
                CountryCode = "GB",
                CountryName = "United Kingdom",
                PostalCode = "BB2 2BB"
            },
            Description = "This is an updated test event"
        };

        private Web.Controllers.EventController EventController;

        [SetUp]
        public async Task Setup()
        {
            EventServiceMock.Setup(x => x.Update(UpdateEventBody));
            EventServiceMock.Setup(x => x.Update(null))
                .ThrowsAsync(new EventNotFoundException());
            EventController = new Web.Controllers.EventController(EventServiceMock.Object,
                new Mock<ILogger<Web.Controllers.EventController>>().Object);
        }

        [Test]
        public async Task ShouldUpdate()
        {
            var result = await EventController.Update(UpdateEventBody);
            result.Should().BeOfType<OkResult>();
        }

        [Test]
        public async Task ShouldCatchException()
        {
            var result = await EventController.Update(null);
            result.Should()
                .BeOfType<BadRequestObjectResult>()
                .Subject.Value.Should().BeOfType<EventNotFoundException>();
        }
    }
}
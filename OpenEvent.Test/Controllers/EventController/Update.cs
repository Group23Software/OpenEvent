using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using OpenEvent.Data.Models.Address;
using OpenEvent.Data.Models.Event;
using OpenEvent.Web;
using OpenEvent.Web.Exceptions;
using OpenEvent.Web.Services;

namespace OpenEvent.Test.Controllers.EventController
{
    [TestFixture]
    public class Update
    {
        private readonly Mock<IEventService> EventServiceMock = new();
        private readonly Mock<IRecommendationService> RecommendationServiceMock = new();
        private readonly Mock<IWorkQueue> WorkQueueMock = new();

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

        private UpdateEventBody ErrorBody = new UpdateEventBody() { };

        private Web.Controllers.EventController EventController;

        [SetUp]
        public async Task Setup()
        {
            EventServiceMock.Setup(x => x.Update(UpdateEventBody));
            EventServiceMock.Setup(x => x.Update(ErrorBody)).ThrowsAsync(new EventNotFoundException());
            
            EventController = new Web.Controllers.EventController(
                EventServiceMock.Object,
                new Mock<ILogger<Web.Controllers.EventController>>().Object, 
                RecommendationServiceMock.Object,
                WorkQueueMock.Object);
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
            var result = await EventController.Update(ErrorBody);
            result.Should()
                .BeOfType<BadRequestObjectResult>()
                .Subject.Value.Should().BeOfType<EventNotFoundException>();
        }
    }
}
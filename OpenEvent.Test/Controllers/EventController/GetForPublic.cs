using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using OpenEvent.Web;
using OpenEvent.Web.Exceptions;
using OpenEvent.Web.Models.Event;
using OpenEvent.Web.Services;
using IEventService = OpenEvent.Web.Services.IEventService;

namespace OpenEvent.Test.Controllers.EventController
{
    [TestFixture]
    public class GetForPublic
    {
        private readonly Mock<IEventService> EventServiceMock = new();
        private readonly Mock<IRecommendationService> RecommendationServiceMock = new();
        private readonly Mock<IWorkQueue> WorkQueueMock = new();
        
        private readonly EventDetailModel TestEvent = new()
        {
            Id = new Guid("361F25C7-6F93-463F-AA8C-C6976D5AEDEC")
        };

        private Web.Controllers.EventController EventController;

        [SetUp]
        public async Task Setup()
        {
            EventServiceMock.Setup(x => x.GetForPublic(TestEvent.Id,new Guid())).ReturnsAsync(TestEvent);
            EventServiceMock.Setup(x => x.GetForPublic(new Guid(),new Guid())).ThrowsAsync(new EventNotFoundException());
            EventController = new Web.Controllers.EventController(
                EventServiceMock.Object,
                new Mock<ILogger<Web.Controllers.EventController>>().Object, 
                RecommendationServiceMock.Object,
                WorkQueueMock.Object);
        }

        [Test]
        public async Task ShouldReturnEventForPublic()
        {
            var result = await EventController.GetForPublic(TestEvent.Id,new Guid());
            result.Should().BeOfType<ActionResult<EventDetailModel>>();
            result.Value.Id.Should().Be(TestEvent.Id);
        }

        [Test]
        public async Task ShouldCatchException()
        {
            var result = await EventController.GetForPublic(new Guid(),new Guid());
            result.Result.Should()
                .BeOfType<BadRequestObjectResult>()
                .Subject.Value.Should().BeOfType<EventNotFoundException>();
        }
    }
}
using System;
using System.Threading.Tasks;
using FluentAssertions;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using OpenEvent.Web.Exceptions;
using OpenEvent.Web.Models.Event;
using IEventService = OpenEvent.Web.Services.IEventService;

namespace OpenEvent.Test.Controllers.EventController
{
    [TestFixture]
    public class GetForPublic
    {
        private readonly Mock<Web.Services.IEventService> EventServiceMock = new();

        private readonly EventDetailModel TestEvent = new()
        {
            Id = new Guid("361F25C7-6F93-463F-AA8C-C6976D5AEDEC")
        };

        private Web.Controllers.EventController EventController;

        [SetUp]
        public async Task Setup()
        {
            EventServiceMock.Setup(x => x.GetForPublic(TestEvent.Id)).ReturnsAsync(TestEvent);
            EventServiceMock.Setup(x => x.GetForPublic(new Guid())).ThrowsAsync(new EventNotFoundException());
            EventController = new Web.Controllers.EventController(EventServiceMock.Object,
                new Mock<ILogger<Web.Controllers.EventController>>().Object);
        }

        [Test]
        public async Task ShouldReturnEventForPublic()
        {
            var result = await EventController.GetForPublic(TestEvent.Id);
            result.Should().BeOfType<ActionResult<EventDetailModel>>();
            result.Value.Id.Should().Be(TestEvent.Id);
        }

        [Test]
        public async Task ShouldCatchException()
        {
            var result = await EventController.GetForPublic(new Guid());
            result.Result.Should()
                .BeOfType<BadRequestObjectResult>()
                .Subject.Value.Should().BeOfType<EventNotFoundException>();
        }
    }
}
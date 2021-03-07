using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using OpenEvent.Web.Exceptions;
using OpenEvent.Web.Services;

namespace OpenEvent.Test.Controllers.EventController
{
    [TestFixture]
    public class Cancel
    {
        private readonly Mock<IEventService> EventServiceMock = new();
        private readonly Mock<IRecommendationService> RecommendationServiceMock = new();

        private readonly Guid EventId = new Guid("68933B68-4D0F-44B3-B7C4-CAB80AE97F31");
        private readonly Guid SaveErrorId = new Guid("BEB7C944-EC49-43AA-BAAC-94BD5825556D");

        private Web.Controllers.EventController EventController;

        [SetUp]
        public async Task Setup()
        {
            EventServiceMock.Setup(x => x.Cancel(EventId));
            EventServiceMock.Setup(x => x.Cancel(new Guid())).ThrowsAsync(new EventNotFoundException());
            EventServiceMock.Setup(x => x.Cancel(SaveErrorId)).ThrowsAsync(new DbUpdateException());
            EventController = new Web.Controllers.EventController(EventServiceMock.Object,
                new Mock<ILogger<Web.Controllers.EventController>>().Object,RecommendationServiceMock.Object);
        }

        [Test]
        public async Task ShouldCancel()
        {
            var result = await EventController.Cancel(EventId);
            result.Should().BeOfType<OkResult>();
        }

        [Test]
        public async Task ShouldNotFindUser()
        {
            var result = await EventController.Cancel(new Guid());
            result.Should()
                .BeOfType<BadRequestObjectResult>()
                .Subject.Value.Should().BeOfType<EventNotFoundException>();
        }
        
        [Test]
        public async Task ShouldThrowDbUpdateException()
        {
            var result = await EventController.Cancel(SaveErrorId);
            result.Should()
                .BeOfType<BadRequestObjectResult>()
                .Subject.Value.Should().BeOfType<DbUpdateException>();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using OpenEvent.Web.Exceptions;
using OpenEvent.Web.Models.Category;
using OpenEvent.Web.Models.Event;
using OpenEvent.Web.Models.Ticket;

namespace OpenEvent.Test.Controllers.EventController
{
    [TestFixture]
    public class GetForHost
    {
        private readonly Mock<Web.Services.IEventService> EventServiceMock = new();

        private readonly EventHostModel TestData = new()
        {
            Id = new Guid("5F6FFE6E-7A50-456F-838E-910C0C89D16F")
        };

        private Web.Controllers.EventController EventController;

        [SetUp]
        public async Task Setup()
        {
            EventServiceMock.Setup(x => x.GetForHost(TestData.Id)).ReturnsAsync(TestData);
            EventServiceMock.Setup(x => x.GetForHost(new Guid())).ThrowsAsync(new EventNotFoundException());
            EventController = new Web.Controllers.EventController(EventServiceMock.Object,
                new Mock<ILogger<Web.Controllers.EventController>>().Object);
        }
        
        
        [Test]
        public async Task ShouldGetForHost()
        {
            var result = await EventController.GetForHost(TestData.Id);
            result.Should().BeOfType<ActionResult<EventHostModel>>();
        }
        
        [Test]
        public async Task ShouldCatchException()
        {
            var result = await EventController.GetForHost(new Guid());
            result.Result.Should()
                .BeOfType<BadRequestObjectResult>()
                .Subject.Value.Should().BeOfType<EventNotFoundException>();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using OpenEvent.Data.Models.Event;
using OpenEvent.Web;
using OpenEvent.Web.Services;

namespace OpenEvent.Test.Controllers.EventController
{
    [TestFixture]
    public class Search
    {
        private readonly Mock<IEventService> EventServiceMock = new();
        private readonly Mock<IRecommendationService> RecommendationServiceMock = new();
        private readonly Mock<IWorkQueue> WorkQueueMock = new();

        private Web.Controllers.EventController EventController;
        
        List<SearchFilter> searchFilters = new List<SearchFilter>();

        [SetUp]
        public async Task Setup()
        {
            // EventServiceMock.Setup(x => x.GetForHost(TestData.Id)).ReturnsAsync(TestData);
            EventServiceMock.Setup(x => x.Search("",searchFilters,new Guid())).ThrowsAsync(new Exception("Error searching"));
            EventController = new Web.Controllers.EventController(
                EventServiceMock.Object,
                new Mock<ILogger<Web.Controllers.EventController>>().Object, 
                RecommendationServiceMock.Object,
                WorkQueueMock.Object);
        }

        [Test]
        public async Task Should_Search()
        {
            List<SearchFilter> searchFilters = new List<SearchFilter>();

            var result = await EventController.Search("", searchFilters,new Guid());
            result.Should().BeOfType<ActionResult<List<EventViewModel>>>();
        }
        
        [Test]
        public async Task Should_Catch_Exception()
        {
            var result = await EventController.Search("", searchFilters,new Guid());
            result.Result.Should().BeOfType<BadRequestObjectResult>().Subject.Value.Should().BeOfType<Exception>();
        }
    }
}
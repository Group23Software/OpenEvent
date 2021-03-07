using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using OpenEvent.Web.Models.Event;
using OpenEvent.Web.Services;

namespace OpenEvent.Test.Controllers.EventController
{
    [TestFixture]
    public class Search
    {
        private readonly Mock<Web.Services.IEventService> EventServiceMock = new();
        private readonly Mock<IRecommendationService> RecommendationServiceMock = new();

        private Web.Controllers.EventController EventController;

        [SetUp]
        public async Task Setup()
        {
            // EventServiceMock.Setup(x => x.GetForHost(TestData.Id)).ReturnsAsync(TestData);
            EventServiceMock.Setup(x => x.Search(null,null,new Guid())).ThrowsAsync(new Exception("Error searching"));
            EventController = new Web.Controllers.EventController(EventServiceMock.Object,
                new Mock<ILogger<Web.Controllers.EventController>>().Object,RecommendationServiceMock.Object);
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
            List<SearchFilter> searchFilters = new List<SearchFilter>();

            var result = await EventController.Search(null, null,new Guid());
            result.Result.Should().BeOfType<BadRequestObjectResult>().Subject.Value.Should().BeOfType<Exception>();
        }
    }
}
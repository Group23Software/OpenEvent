using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using OpenEvent.Web;
using OpenEvent.Web.Models.Category;
using OpenEvent.Web.Services;

namespace OpenEvent.Test.Controllers.EventController
{
    [TestFixture]
    public class GetAllCategories
    {
        private readonly Mock<IEventService> EventServiceMock = new();
        private readonly Mock<IRecommendationService> RecommendationServiceMock = new();
        private readonly Mock<IWorkQueue> WorkQueueMock = new();

        private readonly Guid HostId = new Guid("DDA81D6C-0FCE-49CF-87C6-CB92761347D1");

        private readonly List<Category> TestData = new()
        {
            new Category(),
            new Category()
        };

        private Web.Controllers.EventController EventController;

        [SetUp]
        public async Task Setup()
        {
            EventServiceMock.Setup(x => x.GetAllCategories()).ReturnsAsync(TestData);
            EventController = new Web.Controllers.EventController(
                EventServiceMock.Object,
                new Mock<ILogger<Web.Controllers.EventController>>().Object, 
                RecommendationServiceMock.Object,
                WorkQueueMock.Object);
        }
        
        [Test]
        public async Task ShouldGetALlCategories()
        {
            var result = await EventController.GetAllCategories();
            result.Should().BeOfType<ActionResult<List<Category>>>();
        }
    }
}
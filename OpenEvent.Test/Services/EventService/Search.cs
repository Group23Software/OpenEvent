using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using OpenEvent.Web.Contexts;
using OpenEvent.Web.Models.Event;

namespace OpenEvent.Test.Services.EventService
{
    [TestFixture]
    public class Search : EventTestFixture
    {
        [Test]
        public async Task Should_Search_By_Keyword()
        {
            // await MockContext.Object.Events.AddAsync(TestData.TestEventData.FakeEvent.Generate(10));
            var result = await EventService.Search("Test", new List<SearchFilter>());
            result.Count().Should().BeGreaterThan(0);
            result.ForEach(r => r.Name.Should().Contain("Test"));
        }

        [Test]
        public async Task Should_Search_By_Category()
        {
            var searchFilters = new List<SearchFilter>()
            {
                new SearchFilter()
                {
                    Key = SearchParam.Category,
                    Value = "534DE110-2D1D-4AE8-9293-68FC8037DB5A" // Music category
                }
            };
            var result = await EventService.Search("Test", searchFilters);
            result.Count().Should().Be(1);
        }

        [Test]
        public async Task Null_Keyword_Should_Return_All()
        {
            var result = await EventService.Search(null, new List<SearchFilter>());
            result.Should().NotBeNull();
            result.Count().Should().BeGreaterThan(0);
        }

        [Test]
        public async Task Should_Limit_Results_To_50()
        {
            await MockContext.Object.Events.AddRangeAsync(TestData.TestEventData.FakeEvent.Generate(100).ToList());
            await MockContext.Object.SaveChangesAsync();
            var result = await EventService.Search("", new List<SearchFilter>());
            result.Should().NotBeNull();
            result.Count().Should().Be(50);
        }

        [Test]
        public async Task Should_Only_Return_Online_Events()
        {
            var result = await EventService.Search("Test", new List<SearchFilter>(){new(){Key = SearchParam.IsOnline,Value = "true"}});
            result.Count.Should().BePositive();
            result.ForEach(e => e.IsOnline.Should().BeTrue());
        }

        [Test]
        public async Task Should_Search_By_Location()
        {
            var e = TestData.TestEventData.FakeEvent.Generate();
            e.Address.Lat = 51.47353;
            e.Address.Lon = -0.08069;
            await MockContext.Object.Events.AddAsync(e);
            await MockContext.Object.SaveChangesAsync();
            var result = await EventService.Search("",
                new List<SearchFilter>() {new() {Key = SearchParam.Location, Value = "51.47338,-0.08375,1000"}});
            result.Should().NotBeNull();
            result.ForEach(x => x.IsOnline.Should().BeFalse());
        }
    }
}
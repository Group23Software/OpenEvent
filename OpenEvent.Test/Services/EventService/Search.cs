using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using OpenEvent.Data;
using OpenEvent.Data.Models.Event;
using OpenEvent.Test.Factories;
using OpenEvent.Test.Setups;

namespace OpenEvent.Test.Services.EventService
{
    [TestFixture]
    public class Search
    {
        [Test]
        public async Task Should_Search_By_Keyword()
        {
            await using (var context = new DbContextFactory().CreateContext())
            {
                var service = new EventServiceFactory().Create(context);

                var result = await service.Search("Test", new List<SearchFilter>(), new Guid());
                result.Count().Should().BeGreaterThan(0);
                result.ForEach(r => r.Name.Should().Contain("Test"));
            }
        }

        [Test]
        public async Task Should_Search_By_Category()
        {
            await using (var context = new DbContextFactory().CreateContext())
            {
                var service = new EventServiceFactory().Create(context);

                var categoryId = context.Categories.First().Id;

                var searchFilters = new List<SearchFilter>()
                {
                    new()
                    {
                        Key = SearchParam.Category,
                        Value = categoryId.ToString() // Music category
                    }
                };
                var result = await service.Search("", searchFilters, new Guid());
                result.Count().Should().Be(1);
            }
        }

        [Test]
        public async Task Null_Keyword_Should_Return_All()
        {
            await using (var context = new DbContextFactory().CreateContext())
            {
                var service = new EventServiceFactory().Create(context);

                var result = await service.Search(null, new List<SearchFilter>(), new Guid());
                result.Should().NotBeNull();
                result.Count.Should().BeGreaterThan(2);
            }
        }

        [Test]
        public async Task Should_Limit_Results_To_50()
        {
            await using (var context = new DbContextFactory().CreateContext())
            {
                var service = new EventServiceFactory().Create(context);

                await context.Events.AddRangeAsync(EventData.FakeEvent.Generate(100).ToList());
                await context.SaveChangesAsync();
                var result = await service.Search("", new List<SearchFilter>(), new Guid());
                result.Should().NotBeNull();
                result.Count().Should().Be(50);
            }
        }

        [Test]
        public async Task Should_Only_Return_Online_Events()
        {
            await using (var context = new DbContextFactory().CreateContext())
            {
                var service = new EventServiceFactory().Create(context);

                var result = await service.Search("",
                    new List<SearchFilter>() {new() {Key = SearchParam.IsOnline, Value = "true"}}, new Guid());
                result.Count.Should().BePositive();
                result.ForEach(e => e.IsOnline.Should().BeTrue());
            }
        }

        [Test]
        public async Task Should_Search_By_Location()
        {
            await using (var context = new DbContextFactory().CreateContext())
            {
                var service = new EventServiceFactory().Create(context);

                var e = EventData.FakeEvent.Generate();
                e.Address.Lat = 51.47353;
                e.Address.Lon = -0.08069;
                await context.Events.AddAsync(e);
                await context.SaveChangesAsync();
                var result = await service.Search("",
                    new List<SearchFilter> {new() {Key = SearchParam.Location, Value = "51.47338,-0.08375,1000"}},
                    new Guid());
                result.Should().NotBeNull();
                result.ForEach(x => x.IsOnline.Should().BeFalse());
            }
        }

        [Test]
        public async Task Should_Search_By_Date()
        {
            await using (var context = new DbContextFactory().CreateContext())
            {
                var service = new EventServiceFactory().Create(context);

                var e = EventData.FakeEvent.Generate();
                e.StartLocal = new DateTime(0);
                await context.Events.AddAsync(e);
                await context.SaveChangesAsync();
                var result = await service.Search("",
                    new List<SearchFilter> {new() {Key = SearchParam.Date, Value = new DateTime(0).ToString()}},
                    new Guid());
                result.Should().NotBeNull();
                result.First().StartLocal.Should().Equals(new DateTime(0));
            }
        }
    }
}
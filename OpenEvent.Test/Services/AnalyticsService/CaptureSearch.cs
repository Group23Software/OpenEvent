using System;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using OpenEvent.Test.Factories;

namespace OpenEvent.Test.Services.AnalyticsService
{
    [TestFixture]
    public class CaptureSearch
    {
        [Test]
        public async Task Should_Capture()
        {
            await using (var context = new DbContextFactory().CreateContext())
            {
                var service = new AnalyticsServiceFactory().Create(context);
                
                await service.CaptureSearchAsync(CancellationToken.None, "UniqueSearch", "SearchParam", new Guid("046E876E-D413-45AF-AC2A-552D7AA46C5C"),DateTime.Now);
                
                var searchEvents = context.SearchEvents.AsQueryable();
                searchEvents.Should().NotContainNulls();
                searchEvents.Should().ContainSingle(x => x.Search == "UniqueSearch" && x.Params == "SearchParam");
            }
        }
    }
}
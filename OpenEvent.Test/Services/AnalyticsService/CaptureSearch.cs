using System;
using System.Data.Entity;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using OpenEvent.Test.Factories;
using OpenEvent.Test.Setups;

namespace OpenEvent.Test.Services.AnalyticsService
{
    [TestFixture]
    public class CaptureSearch
    {
        // [Test]
        // public async Task Should_Capture()
        // {
        //     await using (var context = new DbContextFactory().CreateContext())
        //     {
        //         var service = new AnalyticsServiceFactory().Create(context);
        //         
        //         service.CaptureSearch("UniqueSearch", "SearchParam", new Guid("046E876E-D413-45AF-AC2A-552D7AA46C5C"));
        //         await Task.Delay(1);
        //         var searchEvent = await context.SearchEvents.FirstOrDefaultAsync(x => x.Search == "UniqueSearch" && x.Params == "SearchParam");
        //         searchEvent.Should().NotBeNull();
        //     }
        // }
    }
}
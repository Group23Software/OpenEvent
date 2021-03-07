using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using OpenEvent.Test.Factories;
using OpenEvent.Test.Setups;
using OpenEvent.Web.Contexts;

namespace OpenEvent.Test.Services.AnalyticsService
{
    [TestFixture]
    public class CapturePageView
    {
        // private readonly Guid UserId = new Guid("046E876E-D413-45AF-AC2A-552D7AA46C5C");
        //
        // [Test]
        // public async Task Should_Capture()
        // {
        //     await using (var context = new DbContextFactory().CreateContext())
        //     {
        //         var service = new AnalyticsServiceFactory().Create(context);
        //
        //         service.CapturePageView(new Guid("74831876-FC2E-4D03-99D8-B3872BDEFD5C"), UserId);
        //         // await Task.Delay(1000);
        //         // using var scope = ServiceScopeFactoryMock.Object.CreateScope();
        //         // var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
        //         // var pageView = await context.PageViewEvents.FirstOrDefaultAsync(x => x.Id == UserId);
        //         // pageView.Should().NotBeNull();
        //     }
        // }
    }
}
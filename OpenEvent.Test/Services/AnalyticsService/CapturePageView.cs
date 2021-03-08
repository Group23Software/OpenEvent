using System;
using System.Data.Entity;
using System.Linq;
using System.Threading;
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
        [Test]
        public async Task Should_Capture()
        {
            await using (var context = new DbContextFactory().CreateContext())
            {
                var service = new AnalyticsServiceFactory().Create(context);

                var created = DateTime.Now.AddHours(2);
                var eventId = new Guid("74831876-FC2E-4D03-99D8-B3872BDEFD5C");
                var userId = new Guid("046E876E-D413-45AF-AC2A-552D7AA46C5C");

                await service.CapturePageViewAsync(CancellationToken.None, eventId, userId, created);

                var pageViews = context.PageViewEvents.AsQueryable();
                pageViews.Should().NotContainNulls();
                pageViews.Should()
                    .ContainSingle(x => x.Created == created && x.Event.Id == eventId && x.User.Id == userId);
            }
        }
    }
}
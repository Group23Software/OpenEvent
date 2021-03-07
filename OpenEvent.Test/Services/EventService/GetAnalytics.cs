using System;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using OpenEvent.Test.Factories;
using OpenEvent.Test.Setups;

namespace OpenEvent.Test.Services.EventService
{
    [TestFixture]
    public class GetAnalytics
    {
        [Test]
        public async Task Should_Get_Analytics()
        {
            await using (var context = new DbContextFactory().CreateContext())
            {
                var service = new EventServiceFactory().Create(context);
                
                var result = await service.GetAnalytics(new Guid("74831876-FC2E-4D03-99D8-B3872BDEFD5C"));
                result.Should().NotBeNull();
                // result.AverageRecommendationScores.Should().NotContainNulls().And.BeInDescendingOrder();
                result.PageViewEvents.Should().NotContainNulls().And.BeInDescendingOrder(x => x.Created);
                result.TicketVerificationEvents.Should().NotContainNulls().And.BeInDescendingOrder(x => x.Created);
            }
        }
    }
}
using System;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using OpenEvent.Test.Factories;
using OpenEvent.Test.Setups;

namespace OpenEvent.Test.Services.UserService
{
    [TestFixture]
    public class GetUsersAnalytics
    {

        [Test]
        public async Task Should_Get_Users_Analytics()
        {
            await using (var context = new DbContextFactory().CreateContext())
            {
                var service = new UserServiceFactory().Create(context);

                var result = await service.GetUsersAnalytics(new Guid("046E876E-D413-45AF-AC2A-552D7AA46C5C"));

                result.Should().NotBeNull();

                result.RecommendationScores.Should().NotContainNulls();
                result.SearchEvents.Should().NotContainNulls().And.BeInDescendingOrder(x => x.Created);
                result.PageViewEvents.Should().NotContainNulls().And.BeInDescendingOrder(x => x.Created);
                result.TicketVerificationEvents.Should().NotContainNulls().And.BeInDescendingOrder(x => x.Created);
            }
        }
    }
}
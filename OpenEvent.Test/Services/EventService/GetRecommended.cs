using System;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using OpenEvent.Test.Factories;

namespace OpenEvent.Test.Services.EventService
{
    [TestFixture]
    public class GetRecommended
    {
        [Test]
        public async Task Should_Get_Recommended()
        {
            await using (var context = new DbContextFactory().CreateContext())
            {
                var service = new EventServiceFactory().Create(context);

                var result = await service.GetRecommended(new Guid("046E876E-D413-45AF-AC2A-552D7AA46C5C"));

                result.Should().NotBeNull();
                result.Should().NotContainNulls();
            }
        }
    }
}
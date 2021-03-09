using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using OpenEvent.Test.Factories;

namespace OpenEvent.Test.Services.PromoService
{
    [TestFixture]
    public class Create
    {
        [Test]
        public async Task Should_Create()
        {
            await using (var context = new DbContextFactory().CreateContext())
            {
                var service = new PromoServiceFactory().Create(context);

                var createPromoBody = TestData.FakeCreatePromoBody.Generate();
                var e = await context.Events.FirstAsync();
                e.Should().NotBeNull();
                createPromoBody.EventId = e.Id;

                var result = await service.Create(createPromoBody);
                result.Should().NotBeNull();

                var check = await context.Promos.FirstOrDefaultAsync(x => x.Id == result.Id);
                check.Should().NotBeNull();
            }
        }
    }
}
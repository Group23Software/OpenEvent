using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using OpenEvent.Test.Factories;
using OpenEvent.Web.Exceptions;

namespace OpenEvent.Test.Services.PromoService
{
    [TestFixture]
    public class Destroy
    {
        [Test]
        public async Task Should_Destroy()
        {
            await using (var context = new DbContextFactory().CreateContext())
            {
                var service = new PromoServiceFactory().Create(context);

                var promo = await context.Promos.FirstAsync();
                promo.Should().NotBeNull();

                await service.Destroy(promo.Id);

                var check = await context.Promos.FirstOrDefaultAsync(x => x.Id == promo.Id);
                check.Should().BeNull();
            }
        }

        [Test]
        public async Task Should_Not_Find_Promo()
        {
            await using (var context = new DbContextFactory().CreateContext())
            {
                var service = new PromoServiceFactory().Create(context);

                FluentActions.Invoking(async () => await service.Destroy(new Guid()))
                    .Should().Throw<PromoNotFoundException>();
            }
        }
    }
}
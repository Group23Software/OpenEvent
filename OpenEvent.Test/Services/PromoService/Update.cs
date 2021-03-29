using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using OpenEvent.Data.Models.Promo;
using OpenEvent.Test.Factories;
using OpenEvent.Web.Exceptions;

namespace OpenEvent.Test.Services.PromoService
{
    [TestFixture]
    public class Update
    {
        [Test]
        public async Task Should_Update()
        {
            await using (var context = new DbContextFactory().CreateContext())
            {
                var service = new PromoServiceFactory().Create(context);

                var promo = await context.Promos.FirstAsync();
                promo.Should().NotBeNull();

                var newDate = promo.End.AddYears(1);

                var result = await service.Update(new UpdatePromoBody()
                {
                    Id = promo.Id,
                    Active = promo.Active,
                    Discount = promo.Discount,
                    End = newDate,
                    Start = promo.Start
                });

                result.Should().NotBeNull();

                var check = await context.Promos.FirstOrDefaultAsync(x => x.Id == promo.Id);
                check.Should().NotBeNull();
                check.End.Should().Be(newDate);
            }
        }
        
        [Test]
        public async Task Should_Not_Find_Promo()
        {
            await using (var context = new DbContextFactory().CreateContext())
            {
                var service = new PromoServiceFactory().Create(context);

                FluentActions.Invoking(async () => await service.Update(new UpdatePromoBody()))
                    .Should().Throw<PromoNotFoundException>();
            }
        }
    }
}
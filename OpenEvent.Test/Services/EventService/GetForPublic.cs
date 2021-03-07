using System;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using OpenEvent.Test.Factories;
using OpenEvent.Test.Setups;
using OpenEvent.Web.Exceptions;
using OpenEvent.Web.Models.Event;

namespace OpenEvent.Test.Services.EventService
{
    [TestFixture]
    public class GetForPublic
    {
        private readonly Guid RealEventId = new Guid("74831876-FC2E-4D03-99D8-B3872BDEFD5C");

        [Test]
        public async Task ShouldGetForPublic()
        {
            await using (var context = new DbContextFactory().CreateContext())
            {
                var service = new EventServiceFactory().Create(context);

                var result = await service.GetForPublic(RealEventId, new Guid());
                result.Should().NotBeNull();
                result.Should().BeOfType<EventDetailModel>();
            }
        }

        [Test]
        public async Task ShouldNotFindEvent()
        {
            await using (var context = new DbContextFactory().CreateContext())
            {
                var service = new EventServiceFactory().Create(context);
                
                FluentActions.Invoking(async () => await service.GetForPublic(new Guid(), new Guid()))
                    .Should().Throw<EventNotFoundException>();
            }
        }
    }
}
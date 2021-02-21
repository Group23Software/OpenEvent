using System;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using OpenEvent.Web.Exceptions;
using OpenEvent.Web.Models.Event;

namespace OpenEvent.Test.Services.EventService
{
    [TestFixture]
    public class GetForPublic : EventTestFixture
    {
        private readonly Guid RealEventId = new Guid("74831876-FC2E-4D03-99D8-B3872BDEFD5C");
        
        [Test]
        public async Task ShouldGetForPublic()
        {
            var result = await EventService.GetForPublic(RealEventId, new Guid());
            result.Should().NotBeNull();
            result.Should().BeOfType<EventDetailModel>();
        }
        
        [Test]
        public async Task ShouldNotFindEvent()
        {
            FluentActions.Invoking(async () => await EventService.GetForPublic(new Guid(), new Guid()))
                .Should().Throw<EventNotFoundException>();
        }
    }
}
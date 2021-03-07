using System;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using OpenEvent.Test.Factories;
using OpenEvent.Test.Setups;

namespace OpenEvent.Test.Services.UserService
{
    [TestFixture]
    public class HostOwnsEvent
    {
        private readonly Guid EventId = new Guid("74831876-FC2E-4D03-99D8-B3872BDEFD5C");
        private readonly Guid UserId = new Guid("046E876E-D413-45AF-AC2A-552D7AA46C5C");
        
        [Test]
        public async Task ShouldOwnEvent()
        {
            await using (var context = new DbContextFactory().CreateContext())
            {
                var service = new UserServiceFactory().Create(context);

                var result = await service.HostOwnsEvent(EventId, UserId);
                result.Should().Be(true);
            }
        }
        
        [Test]
        public async Task ShouldNotOwnEvent()
        {
            await using (var context = new DbContextFactory().CreateContext())
            {
                var service = new UserServiceFactory().Create(context);

                var result = await service.HostOwnsEvent(EventId, new Guid());
                result.Should().Be(false);
            }
        }
    }
}
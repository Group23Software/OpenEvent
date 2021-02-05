using System;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace OpenEvent.Test.Services.UserService
{
    [TestFixture]
    public class HostOwnsEvent : UserTestFixture
    {
        private readonly Guid EventId = new Guid("74831876-FC2E-4D03-99D8-B3872BDEFD5C");
        private readonly Guid UserId = new Guid("046E876E-D413-45AF-AC2A-552D7AA46C5C");
        
        [Test]
        public async Task ShouldOwnEvent()
        {
            var result = await UserService.HostOwnsEvent(EventId,UserId);
            result.Should().Be(true);
        }
        
        [Test]
        public async Task ShouldNotOwnEvent()
        {
            var result = await UserService.HostOwnsEvent(EventId,new Guid());
            result.Should().Be(false);
        }
    }
}
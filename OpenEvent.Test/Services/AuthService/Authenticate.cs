using System;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using OpenEvent.Web.Exceptions;

namespace OpenEvent.Test.Services.AuthService
{
    [TestFixture]
    public class Authenticate : AuthTestFixture
    {
        [Test]
        public async Task ShouldLogin()
        {
            var result = await AuthService.Authenticate(new Guid("046E876E-D413-45AF-AC2A-552D7AA46C5C"));
            result.Should().NotBeNull();
        }

        [Test]
        public async Task ShouldNotFind()
        {
            FluentActions.Invoking(async () => await AuthService.Authenticate(new Guid()))
                .Should().Throw<UserNotFoundException>();
        }
    }
}
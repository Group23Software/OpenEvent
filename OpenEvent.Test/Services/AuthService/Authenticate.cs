using System;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using OpenEvent.Test.Factories;
using OpenEvent.Test.Setups;
using OpenEvent.Web.Exceptions;

namespace OpenEvent.Test.Services.AuthService
{
    [TestFixture]
    public class Authenticate
    {
        [Test]
        public async Task ShouldLogin()
        {
            await using (var context = new DbContextFactory().CreateContext())
            {
                var service = new AuthServiceFactory().Create(context);

                var result = await service.Authenticate(new Guid("046E876E-D413-45AF-AC2A-552D7AA46C5C"));
                result.Should().NotBeNull();
            }
        }

        [Test]
        public async Task ShouldNotFind()
        {
            await using (var context = new DbContextFactory().CreateContext())
            {
                var service = new AuthServiceFactory().Create(context);

                FluentActions.Invoking(async () => await service.Authenticate(new Guid()))
                    .Should().Throw<UserNotFoundException>();
            }
        }
    }
}
using System;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using OpenEvent.Test.Factories;
using OpenEvent.Test.Setups;
using OpenEvent.Web.Exceptions;

namespace OpenEvent.Test.Services.UserService
{
    [TestFixture]
    public class Get
    {
        [Test]
        public async Task ShouldGetUser()
        {
            await using (var context = new DbContextFactory().CreateContext())
            {
                var service = new UserServiceFactory().Create(context);

                var result = service.Get(new Guid("046E876E-D413-45AF-AC2A-552D7AA46C5C"));

                result.Should().NotBeNull();
            }
        }

        [Test]
        public async Task ShouldNotFindUser()
        {
            await using (var context = new DbContextFactory().CreateContext())
            {
                var service = new UserServiceFactory().Create(context);

                FluentActions.Invoking(async () => await service.Get(new Guid()))
                    .Should().Throw<UserNotFoundException>();
            }
        }
    }
}
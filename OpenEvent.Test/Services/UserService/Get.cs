using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace OpenEvent.Test.Services.UserService
{
    [TestFixture]
    public class Get : UserTestFixture
    {
        [Test]
        public async Task ShouldGetUser()
        {
            var result = UserService.Get(new Guid("046E876E-D413-45AF-AC2A-552D7AA46C5C"));

            result.Should().NotBeNull();
        }

        [Test]
        public async Task ShouldNotFindUser()
        {
            FluentActions.Invoking(async () => await UserService.Get(new Guid()))
                 .Should().Throw<Exception>()
                 .WithMessage("User not found");
        }
    }
}
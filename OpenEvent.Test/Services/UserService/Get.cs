using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace OpenEvent.Test.Services.UserService
{
    [TestFixture]
    public class Get : BasicTestFixture
    {
        [Test]
        public async Task ShouldGetUser()
        {
            var userService =
                new Web.Services.UserService(Context, new Logger<Web.Services.UserService>(new LoggerFactory()));
            
            var result = userService.Get(new Guid("046E876E-D413-45AF-AC2A-552D7AA46C5C"));

            result.Should().NotBeNull();
        }

        [Test]
        public async Task ShouldNotFindUser()
        {
            var userService =
                new Web.Services.UserService(Context, new Logger<Web.Services.UserService>(new LoggerFactory()));

             FluentActions.Invoking(async () => await userService.Get(new Guid()))
                 .Should().Throw<Exception>()
                 .WithMessage("User not found");
        }
    }
}
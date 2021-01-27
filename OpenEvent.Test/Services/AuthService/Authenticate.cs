using System;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace OpenEvent.Test.Services.AuthService
{
    [TestFixture]
    public class Authenticate : AuthTestFixture
    {
        [Test]
        public async Task ShouldAuthenticate()
        {
            var result = await AuthService.Authenticate("email@email.co.uk", "Password", false);
            result.Should().NotBeNull();
        }

        [Test]
        public async Task ShouldBeIncorrect()
        {
            FluentActions.Invoking(async () => await AuthService.Authenticate("email@email.co.uk", "Wrong", false))
                .Should().Throw<Exception>()
                .WithMessage("Password incorrect");
        }

        [Test]
        public async Task ShouldNotExist()
        {
            FluentActions.Invoking(async () => await AuthService.Authenticate("wrong@email.co.uk", "Wrong", false))
                .Should().Throw<Exception>()
                .WithMessage("User does not exist");
        }
    }
}
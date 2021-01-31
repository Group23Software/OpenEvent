using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace OpenEvent.Test.Services.AuthService
{
    [TestFixture]
    public class Login : AuthTestFixture
    {
        [Test]
        public async Task ShouldLogin()
        {
            var result = await AuthService.Login("exists@email.co.uk", "Password", false);
            result.Should().NotBeNull();
        }
        
        [Test]
        public async Task PasswordShouldBeIncorrect()
        {
            FluentActions.Invoking(async () => await AuthService.Login("exists@email.co.uk", "Wrong", false))
                .Should().Throw<Exception>()
                .WithMessage("Incorrect password");
        }
        
        [Test]
        public async Task ShouldNotFind()
        {
            FluentActions.Invoking(async () => await AuthService.Login("wrong@email.co.uk", "Wrong", false))
                .Should().Throw<Exception>()
                .WithMessage("User not found");
        }

        [Test]
        public async Task ShouldReturn30DayToken()
        {
            var result = await AuthService.Login("exists@email.co.uk", "Password", true);
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(result.Token);
            DateTime validFrom = token.ValidFrom;
            DateTime validTo = token.ValidTo;
            validTo.Should().Be(validFrom.AddDays(30));
        }
        
        [Test]
        public async Task ShouldReturn1DayToken()
        {
            var result = await AuthService.Login("exists@email.co.uk", "Password", false);
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(result.Token);
            DateTime validFrom = token.ValidFrom;
            DateTime validTo = token.ValidTo;
            validTo.Should().Be(validFrom.AddDays(1));
        }
    }
}
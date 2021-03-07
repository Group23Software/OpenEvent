using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using OpenEvent.Test.Factories;
using OpenEvent.Test.Setups;
using OpenEvent.Web.Exceptions;

namespace OpenEvent.Test.Services.AuthService
{
    [TestFixture]
    public class Login
    {
        [Test]
        public async Task ShouldLogin()
        {
            await using (var context = new DbContextFactory().CreateContext())
            {
                var service = new AuthServiceFactory().Create(context);

                var result = await service.Login("exists@email.co.uk", "Password", false);
                result.Should().NotBeNull();
            }
        }
        
        [Test]
        public async Task PasswordShouldBeIncorrect()
        {
            await using (var context = new DbContextFactory().CreateContext())
            {
                var service = new AuthServiceFactory().Create(context);

                FluentActions.Invoking(async () => await service.Login("exists@email.co.uk", "Wrong", false))
                    .Should().Throw<IncorrectPasswordException>();
            }
        }
        
        [Test]
        public async Task ShouldNotFind()
        {
            await using (var context = new DbContextFactory().CreateContext())
            {
                var service = new AuthServiceFactory().Create(context);

                FluentActions.Invoking(async () => await service.Login("wrong@email.co.uk", "Wrong", false))
                    .Should().Throw<UserNotFoundException>();
            }
        }

        [Test]
        public async Task ShouldReturn30DayToken()
        {
            await using (var context = new DbContextFactory().CreateContext())
            {
                var service = new AuthServiceFactory().Create(context);

                var result = await service.Login("exists@email.co.uk", "Password", true);
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(result.Token);
                DateTime validFrom = token.ValidFrom;
                DateTime validTo = token.ValidTo;
                validTo.Should().Be(validFrom.AddDays(30));
            }
        }
        
        [Test]
        public async Task ShouldReturn1DayToken()
        {
            await using (var context = new DbContextFactory().CreateContext())
            {
                var service = new AuthServiceFactory().Create(context);

                var result = await service.Login("exists@email.co.uk", "Password", false);
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(result.Token);
                DateTime validFrom = token.ValidFrom;
                DateTime validTo = token.ValidTo;
                validTo.Should().Be(validFrom.AddDays(1));
            }
        }
    }
}
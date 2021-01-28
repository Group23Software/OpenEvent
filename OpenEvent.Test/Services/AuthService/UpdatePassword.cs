using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace OpenEvent.Test.Services.AuthService
{
    [TestFixture]
    public class UpdatePassword : AuthTestFixture
    {
        [Test]
        public async Task ShouldUpdatePassword()
        {
            string email = "exists@email.co.uk";
            
            var user = await Context.Users.FirstOrDefaultAsync(x => x.Email == email);
            var prevPassword = user.Password;
            
            await AuthService.UpdatePassword(email, "NewPassword");

            user.Password.Should().NotBe(prevPassword);
        }

        [Test]
        public async Task ShouldNotFindUser()
        {
            FluentActions.Invoking(async () => await AuthService.UpdatePassword("fail@email.co.uk", "wrong"))
                .Should().Throw<Exception>()
                .WithMessage("User not found");
        }
    }
}
using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using OpenEvent.Web.Exceptions;

namespace OpenEvent.Test.Services.AuthService
{
    [TestFixture]
    public class UpdatePassword : AuthTestFixture
    {
        [Test]
        public async Task ShouldUpdatePassword()
        {
            string email = "exists@email.co.uk";

            var user = await MockContext.Object.Users.FirstOrDefaultAsync(x => x.Email == email);
            var prevPassword = user.Password;

            await AuthService.UpdatePassword(email, "NewPassword");

            user.Password.Should().NotBe(prevPassword);
        }

        [Test]
        public async Task ShouldNotFindUser()
        {
            FluentActions.Invoking(async () => await AuthService.UpdatePassword("fail@email.co.uk", "wrong"))
                .Should().Throw<UserNotFoundException>();
        }

        [Test]
        public async Task ShouldThrowDbUpdateException()
        {
            MockContext.Setup(c => c.SaveChangesAsync(new CancellationToken()))
                .ReturnsAsync(() => throw new DbUpdateException());

            FluentActions.Invoking(async () => await AuthService.UpdatePassword("exists@email.co.uk", "NewPassword"))
                .Should().Throw<DbUpdateException>();
        }
    }
}
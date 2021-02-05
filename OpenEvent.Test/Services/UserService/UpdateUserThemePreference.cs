using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using OpenEvent.Web.Exceptions;

namespace OpenEvent.Test.Services.UserService
{
    [TestFixture]
    public class UpdateUserThemePreference : UserTestFixture
    {
        private readonly Guid ValidUserId = new Guid("046E876E-D413-45AF-AC2A-552D7AA46C5C");
        
        [Test]
        public async Task ShouldUpdate()
        {
            var result =
                await UserService.UpdateThemePreference(ValidUserId, true);
            result.Should().Be(true);

            var check = (await MockContext.Object.Users.FirstOrDefaultAsync(x => x.Id == ValidUserId)).IsDarkMode;
            check.Should().Be(true);
        }

        [Test]
        public async Task ShouldNotFindUser()
        {
            FluentActions.Invoking(async () => await UserService.UpdateThemePreference(Guid.NewGuid(), true))
                .Should().Throw<UserNotFoundException>();
        }

        [Test]
        public async Task ShouldThrowDbUpdateException()
        {
            MockContext.Setup(c => c.SaveChangesAsync(new CancellationToken()))
                .ReturnsAsync(() => throw new DbUpdateException());
            
            FluentActions.Invoking(async () =>
                    await UserService.UpdateThemePreference(ValidUserId, true))
                .Should().Throw<DbUpdateException>();
        }
    }
}
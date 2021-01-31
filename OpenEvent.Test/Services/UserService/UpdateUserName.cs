using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace OpenEvent.Test.Services.UserService
{
    [TestFixture]
    public class UpdateUserName : UserTestFixture
    {
        [Test]
        public async Task ShouldUpdate()
        {
            var result =
                await UserService.UpdateUserName(new Guid("046E876E-D413-45AF-AC2A-552D7AA46C5C"), "UpdatedName");
            result.Should().NotBe("ExistingUser");
            result.Should().Be("UpdatedName");
        }

        [Test]
        public async Task ShouldAlreadyExist()
        {
            FluentActions.Invoking(async () =>
                    await UserService.UpdateUserName(new Guid("046E876E-D413-45AF-AC2A-552D7AA46C5C"), "ExistingUser"))
                .Should().Throw<Exception>()
                .WithMessage("Username already exists");
        }

        [Test]
        public async Task ShouldNotFindUser()
        {
            FluentActions.Invoking(async () => await UserService.UpdateUserName(Guid.NewGuid(), ""))
                .Should().Throw<Exception>()
                .WithMessage("User not found");
        }

        [Test]
        public async Task ShouldThrowDbUpdateException()
        {
            MockContext.Setup(c => c.SaveChangesAsync(new CancellationToken()))
                .ReturnsAsync(() => throw new DbUpdateException());
            
            FluentActions.Invoking(async () =>
                    await UserService.UpdateUserName(new Guid("046E876E-D413-45AF-AC2A-552D7AA46C5C"), "UpdatedName"))
                .Should().Throw<DbUpdateException>();
        }
    }
}
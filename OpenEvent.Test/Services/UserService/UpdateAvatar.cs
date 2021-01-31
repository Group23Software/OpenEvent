using System;
using System.Text;
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
    public class UpdateAvatar : UserTestFixture
    {
        [Test]
        public async Task ShouldUpdate()
        {
            var result = await UserService.UpdateAvatar(new Guid("046E876E-D413-45AF-AC2A-552D7AA46C5C"),
                new byte[] {0, 0, 0, 0});
            result.Should().NotBe(Encoding.UTF8.GetString(new Byte[] {1, 1, 1, 1}, 0, 4));
            result.Should().Be(Encoding.UTF8.GetString(new Byte[] {0, 0, 0, 0}, 0, 4));
        }

        [Test]
        public async Task ShouldNotFindUser()
        {
            FluentActions.Invoking(async () => await UserService.UpdateAvatar(Guid.NewGuid(), null))
                .Should().Throw<UserNotFoundException>();
        }

        [Test]
        public async Task ShouldThrowDbUpdateException()
        {
            MockContext.Setup(c => c.SaveChangesAsync(new CancellationToken()))
                .ReturnsAsync(() => throw new DbUpdateException());

            FluentActions.Invoking(async () =>
                    await UserService.UpdateAvatar(new Guid("046E876E-D413-45AF-AC2A-552D7AA46C5C"),
                        new byte[] {0, 0, 0, 0}))
                .Should().Throw<DbUpdateException>();
        }
    }
}
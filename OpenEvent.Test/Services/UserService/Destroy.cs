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
    public class Destroy : UserTestFixture
    {
        [Test]
        public async Task ShouldDestroyUser()
        {
            var id = new Guid("046E876E-D413-45AF-AC2A-552D7AA46C5C");
            await UserService.Destroy(id);
            var result = await MockContext.Object.Users.FirstOrDefaultAsync(x => x.Id == id);
            result.Should().BeNull();
        }

        [Test]
        public async Task ShouldNotFindUser()
        {
            FluentActions.Invoking(async () => await UserService.Destroy(Guid.NewGuid()))
                .Should().Throw<UserNotFoundException>();
        }
        
        [Test]
        public async Task ShouldThrowDbUpdateException()
        {
            MockContext.Setup(c => c.SaveChangesAsync(new CancellationToken()))
                .ReturnsAsync(() => throw new DbUpdateException());

            FluentActions.Invoking(async () => await UserService.Destroy(new Guid("046E876E-D413-45AF-AC2A-552D7AA46C5C"))).Should().Throw<DbUpdateException>();
        }
    }
}
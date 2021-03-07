using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using OpenEvent.Test.Factories;
using OpenEvent.Test.Setups;
using OpenEvent.Web.Exceptions;

namespace OpenEvent.Test.Services.UserService
{
    [TestFixture]
    public class UpdateUserName
    {
        [Test]
        public async Task ShouldUpdate()
        {
            await using (var context = new DbContextFactory().CreateContext())
            {
                var service = new UserServiceFactory().Create(context);

                var result =
                    await service.UpdateUserName(new Guid("046E876E-D413-45AF-AC2A-552D7AA46C5C"), "UpdatedName");
                result.Should().NotBe("ExistingUser");
                result.Should().Be("UpdatedName");
            }
        }

        [Test]
        public async Task ShouldAlreadyExist()
        {
            await using (var context = new DbContextFactory().CreateContext())
            {
                var service = new UserServiceFactory().Create(context);

                FluentActions.Invoking(async () =>
                        await service.UpdateUserName(new Guid("046E876E-D413-45AF-AC2A-552D7AA46C5C"),
                            "ExistingUser"))
                    .Should().Throw<UserNameAlreadyExistsException>();
            }
        }

        [Test]
        public async Task ShouldNotFindUser()
        {
            await using (var context = new DbContextFactory().CreateContext())
            {
                var service = new UserServiceFactory().Create(context);

                FluentActions.Invoking(async () => await service.UpdateUserName(Guid.NewGuid(), ""))
                    .Should().Throw<UserNotFoundException>();
            }
        }

        // [Test]
        // [Ignore("Need to make separate mock")]
        // public async Task ShouldThrowDbUpdateException()
        // {
        //     MockContext.Setup(c => c.SaveChangesAsync(new CancellationToken()))
        //         .ReturnsAsync(() => throw new DbUpdateException());
        //     
        //     FluentActions.Invoking(async () =>
        //             await UserService.UpdateUserName(new Guid("046E876E-D413-45AF-AC2A-552D7AA46C5C"), "UpdatedName"))
        //         .Should().Throw<DbUpdateException>();
        // }
    }
}
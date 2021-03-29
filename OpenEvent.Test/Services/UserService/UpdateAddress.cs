using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using OpenEvent.Data.Models.Address;
using OpenEvent.Test.Factories;
using OpenEvent.Test.Setups;
using OpenEvent.Web.Exceptions;

namespace OpenEvent.Test.Services.UserService
{
    [TestFixture]
    public class UpdateAddress
    {

        private readonly Guid UserId = new Guid("046E876E-D413-45AF-AC2A-552D7AA46C5C");
        private readonly Address Address = Data.Data.FakeAddress.Generate();
        
        [Test]
        public async Task Should_Update()
        {
            await using (var context = new DbContextFactory().CreateContext())
            {
                var service = new UserServiceFactory().Create(context);

                var result = await service.UpdateAddress(UserId, Address);
                result.Should().Be(Address);
            }
        }
        
        [Test]
        public async Task ShouldNotFindUser()
        {
            await using (var context = new DbContextFactory().CreateContext())
            {
                var service = new UserServiceFactory().Create(context);

                FluentActions.Invoking(async () => await service.UpdateAddress(Guid.NewGuid(), null))
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
        //             await UserService.UpdateAddress(UserId,Address))
        //         .Should().Throw<DbUpdateException>();
        // }
    }
}
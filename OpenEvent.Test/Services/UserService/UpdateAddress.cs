using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using OpenEvent.Web.Exceptions;
using OpenEvent.Web.Models.Address;

namespace OpenEvent.Test.Services.UserService
{
    [TestFixture]
    public class UpdateAddress : UserTestFixture
    {

        private readonly Guid UserId = new Guid("046E876E-D413-45AF-AC2A-552D7AA46C5C");
        private readonly Address Address = TestData.FakeAddress.Generate();
        
        [Test]
        public async Task Should_Update()
        {
            
            var result = await UserService.UpdateAddress(UserId, Address);
            result.Should().Be(Address);
        }
        
        [Test]
        public async Task ShouldNotFindUser()
        {
            FluentActions.Invoking(async () => await UserService.UpdateAddress(Guid.NewGuid(), null))
                .Should().Throw<UserNotFoundException>();
        }

        [Test]
        public async Task ShouldThrowDbUpdateException()
        {
            MockContext.Setup(c => c.SaveChangesAsync(new CancellationToken()))
                .ReturnsAsync(() => throw new DbUpdateException());

            FluentActions.Invoking(async () =>
                    await UserService.UpdateAddress(UserId,Address))
                .Should().Throw<DbUpdateException>();
        }
    }
}
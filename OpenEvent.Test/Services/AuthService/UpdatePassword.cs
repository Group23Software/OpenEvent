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

namespace OpenEvent.Test.Services.AuthService
{
    [TestFixture]
    public class UpdatePassword
    {
        [Test]
        public async Task ShouldUpdatePassword()
        {
            await using (var context = new DbContextFactory().CreateContext())
            {
                var service = new AuthServiceFactory().Create(context);

                Guid id =  new Guid("046E876E-D413-45AF-AC2A-552D7AA46C5C");

                var user = await context.Users.FirstOrDefaultAsync(x => x.Id == id);
                var prevPassword = user.Password;

                await service.UpdatePassword(id, "NewPassword");

                user.Password.Should().NotBe(prevPassword);
            }
        }

        [Test]
        public async Task ShouldNotFindUser()
        {
            await using (var context = new DbContextFactory().CreateContext())
            {
                var service = new AuthServiceFactory().Create(context);

                FluentActions.Invoking(async () => await service.UpdatePassword(Guid.Empty, "wrong"))
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
        //     FluentActions.Invoking(async () => await AuthService.UpdatePassword("exists@email.co.uk", "NewPassword"))
        //         .Should().Throw<DbUpdateException>();
        // }
    }
}
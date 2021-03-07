using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using OpenEvent.Test.Factories;
using OpenEvent.Test.Setups;
using OpenEvent.Web;
using OpenEvent.Web.Contexts;
using OpenEvent.Web.Exceptions;
using OpenEvent.Web.Services;

namespace OpenEvent.Test.Services.UserService
{
    [TestFixture]
    public class Destroy
    {
        [Test]
        public async Task ShouldDestroyUser()
        {
            await using (var context = new DbContextFactory().CreateContext())
            {
                var service = new UserServiceFactory().Create(context);
                
                var id = new Guid("046E876E-D413-45AF-AC2A-552D7AA46C5C");
                await service.Destroy(id);
                var result = await context.Users.FirstOrDefaultAsync(x => x.Id == id);
                result.Should().BeNull();
            }
        }

        [Test]
        public async Task ShouldNotFindUser()
        {
            await using (var context = new DbContextFactory().CreateContext())
            {
                var service = new UserServiceFactory().Create(context);

                FluentActions.Invoking(async () => await service.Destroy(Guid.NewGuid()))
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
        //     FluentActions.Invoking(async () => await UserService.Destroy(new Guid("046E876E-D413-45AF-AC2A-552D7AA46C5C"))).Should().Throw<DbUpdateException>();
        // }
    }
}
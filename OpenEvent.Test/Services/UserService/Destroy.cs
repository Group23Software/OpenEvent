using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using OpenEvent.Test.Setups;
using OpenEvent.Web;
using OpenEvent.Web.Contexts;

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
            var result = await Context.Users.FirstOrDefaultAsync(x => x.Id == id);
            result.Should().BeNull();
        }

        [Test]
        public async Task ShouldNotFindUser()
        {
            FluentActions.Invoking(async () => await UserService.Destroy(Guid.NewGuid()))
                .Should().Throw<Exception>()
                .WithMessage("User doesnt exist");
        }
    }
}
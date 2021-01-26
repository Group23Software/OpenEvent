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
    public class Destroy
    {
        private ApplicationContext Context;
        private IOptions<AppSettings> _appSettings;

        [SetUp]
        public async Task Setup()
        {
            Context = await new BasicSetup().Setup();

            _appSettings = Options.Create(new AppSettings()
            {
                Secret = "this is a secret"
            });
        }

        [TearDown]
        public async Task TearDown()
        {
            await Context.Database.EnsureDeletedAsync();
            await Context.DisposeAsync();
        }

        [Test]
        public async Task ShouldDestroyUser()
        {
            var userService =
                new Web.Services.UserService(Context, null);

            // var result = await userService.Destroy(new Guid("046E876E-D413-45AF-AC2A-552D7AA46C5C"));

            var id = new Guid("046E876E-D413-45AF-AC2A-552D7AA46C5C");
            await userService.Destroy(id);
            var result = await Context.Users.FirstOrDefaultAsync(x => x.Id == id);
            result.Should().BeNull();

            // FluentActions.Invoking(async () => await userService.Destroy(new Guid("046E876E-D413-45AF-AC2A-552D7AA46C5C")))
            //     .Should().NotThrow();
        }

        [Test]
        public async Task ShouldNotFindUser()
        {
            var userService =
                new Web.Services.UserService(Context, new Logger<Web.Services.UserService>(new LoggerFactory()));

            FluentActions.Invoking(async () => await userService.Destroy(Guid.NewGuid()))
                .Should().Throw<Exception>()
                .WithMessage("User doesnt exist");
        }
    }
}
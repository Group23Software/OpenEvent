using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using OpenEvent.Test.Setups;
using OpenEvent.Web;
using OpenEvent.Web.Contexts;

namespace OpenEvent.Test.Services.AuthService
{
    [TestFixture]
    public class Authenticate
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
    }
}
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using OpenEvent.Test.Setups;
using OpenEvent.Web;
using OpenEvent.Web.Contexts;
using OpenEvent.Web.Models;
using OpenEvent.Web.Services;

namespace OpenEvent.Test.Services.AuthService
{
    public class AuthTestFixture
    {
        protected ApplicationContext Context;
        protected IMapper Mapper;
        protected IOptions<AppSettings> AppSettings;
        protected IAuthService AuthService;

        [SetUp]
        public async Task Setup()
        {
            var myProfile = new AutoMapping();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            Mapper = new Mapper(configuration);

            Context = await new BasicSetup().Setup();

            AppSettings = Options.Create(new AppSettings()
            {
                Secret = "this is a secret"
            });

            AuthService = new Web.Services.AuthService(Context,
                new Logger<Web.Services.AuthService>(new LoggerFactory()), AppSettings, Mapper);

        }

        [TearDown]
        public async Task TearDown()
        {
            await Context.Database.EnsureDeletedAsync();
            await Context.DisposeAsync();
        }
    }
}
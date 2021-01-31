using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using OpenEvent.Test.Setups;
using OpenEvent.Web;
using OpenEvent.Web.Contexts;
using OpenEvent.Web.Services;
using ILogger = NUnit.Framework.Internal.ILogger;

namespace OpenEvent.Test
{
    public class BasicTestFixture
    {
        protected ApplicationContext Context;
        protected IMapper Mapper;
        protected IOptions<AppSettings> AppSettings;
        protected IUserService UserService;
        protected IAuthService AuthService;

        [SetUp]
        public async Task Setup()
        {
            var myProfile = new AutoMapping();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            Mapper = new Mapper(configuration);

            // Context = await new BasicSetup().Setup();

            AppSettings = Options.Create(new AppSettings()
            {
                Secret = "this is a secret"
            });

            AuthService = new AuthService(Context,
                new Logger<AuthService>(new LoggerFactory()), AppSettings, Mapper);

            UserService = new UserService(Context,
                new Logger<UserService>(new LoggerFactory()),
                Mapper, AuthService);
        }

        [TearDown]
        public async Task TearDown()
        {
            await Context.Database.EnsureDeletedAsync();
            await Context.DisposeAsync();
        }
    }
}
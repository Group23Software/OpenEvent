using System.Threading.Tasks;
using AutoMapper;
using EntityFrameworkCoreMock;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using OpenEvent.Test.Setups;
using OpenEvent.Web;
using OpenEvent.Web.Contexts;
using OpenEvent.Web.Services;

namespace OpenEvent.Test.Services.UserService
{
    public class UserTestFixture
    {
        // protected ApplicationContext Context;
        protected DbContextMock<ApplicationContext> MockContext;
        protected IMapper Mapper;
        protected IOptions<AppSettings> AppSettings;
        protected IAuthService AuthService;
        protected IUserService UserService;

        [SetUp]
        public async Task Setup()
        {
            var myProfile = new AutoMapping();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            Mapper = new Mapper(configuration);

            MockContext = await new BasicSetup().Setup();

            AppSettings = Options.Create(new AppSettings()
            {
                Secret = "this is a secret"
            });

            AuthService = new Web.Services.AuthService(MockContext.Object,
                new Logger<Web.Services.AuthService>(new LoggerFactory()), AppSettings, Mapper);

            UserService = new Web.Services.UserService(MockContext.Object,
                new Logger<Web.Services.UserService>(new LoggerFactory()),
                Mapper, AuthService);
        }

        [TearDown]
        public async Task TearDown()
        {
            // await MockContext.Object.Database.EnsureDeletedAsync();
            // await MockContext.Object.DisposeAsync();
        }
    }
}
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

namespace OpenEvent.Test.Services.TransactionService
{
    public class TransactionTestFixture
    {
        protected DbContextMock<ApplicationContext> MockContext;
        private IMapper Mapper;
        private IOptions<AppSettings> AppSettings;
        protected ITransactionService TransactionService;

        [SetUp]
        public async Task Setup()
        {
            var myProfile = new AutoMapping();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            Mapper = new Mapper(configuration);

            MockContext = await new BasicSetup().Setup();

            AppSettings = Options.Create(new AppSettings()
            {
                Secret = "this is a secret",
                StripeApiKey = "sk_test_51ILW9dK2ugLXrgQXeYfqg8i0QGAgLXndihLXovHgu47adBimPAedvIwzfr95uffR9TiyleGFAPY7hfSI9mhdmYBF00hkxlAQMv"
            });

            TransactionService = new Web.Services.TransactionService(MockContext.Object,
                new Logger<Web.Services.TransactionService>(new LoggerFactory()),
                Mapper, AppSettings);
        }
    }
}
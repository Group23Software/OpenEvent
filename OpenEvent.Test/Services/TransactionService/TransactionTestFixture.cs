using System;
using System.Threading.Tasks;
using AutoMapper;
using EntityFrameworkCoreMock;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
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
        protected Mock<IServiceScopeFactory> ServiceScopeFactoryMock;
        protected Mock<IRecommendationService> RecommendationServiceMock;
        protected Mock<IEmailService> EmailServiceMock;

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
                StripeApiKey =
                    "sk_test_51ILW9dK2ugLXrgQXeYfqg8i0QGAgLXndihLXovHgu47adBimPAedvIwzfr95uffR9TiyleGFAPY7hfSI9mhdmYBF00hkxlAQMv"
            });

            Mock<IServiceProvider> serviceProvider = new Mock<IServiceProvider>();
            serviceProvider.Setup(x => x.GetService(typeof(ApplicationContext))).Returns(MockContext.Object);

            Mock<IServiceScope> serviceScopeMock = new Mock<IServiceScope>();
            serviceScopeMock.Setup(x => x.ServiceProvider).Returns(() => serviceProvider.Object);

            ServiceScopeFactoryMock = new Mock<IServiceScopeFactory>();
            ServiceScopeFactoryMock.Setup(x => x.CreateScope()).Returns(() => serviceScopeMock.Object);

            RecommendationServiceMock = new Mock<IRecommendationService>();
            EmailServiceMock = new Mock<IEmailService>();

            TransactionService = new Web.Services.TransactionService(
                MockContext.Object,
                new Logger<Web.Services.TransactionService>(new LoggerFactory()),
                Mapper,
                AppSettings,
                ServiceScopeFactoryMock.Object,
                RecommendationServiceMock.Object,
                EmailServiceMock.Object
            );
        }
    }
}
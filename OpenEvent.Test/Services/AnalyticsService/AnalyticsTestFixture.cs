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

namespace OpenEvent.Test.Services.AnalyticsService
{
    public class AnalyticsTestFixture
    {
        protected DbContextMock<ApplicationContext> MockContext;
        protected IMapper Mapper;
        protected IOptions<AppSettings> AppSettings;
        protected IAnalyticsService AnalyticsService;
        protected Mock<IServiceScopeFactory> ServiceScopeFactoryMock;

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
            });

            Mock<IServiceProvider> serviceProvider = new Mock<IServiceProvider>();
            serviceProvider.Setup(x => x.GetService(typeof(ApplicationContext))).Returns(MockContext.Object);
            
            Mock<IServiceScope> serviceScopeMock = new Mock<IServiceScope>();
            serviceScopeMock.Setup(x => x.ServiceProvider).Returns(() => serviceProvider.Object);

            ServiceScopeFactoryMock = new Mock<IServiceScopeFactory>();
            ServiceScopeFactoryMock.Setup(x => x.CreateScope()).Returns(() => serviceScopeMock.Object);

            AnalyticsService = new Web.Services.AnalyticsService(
                new Logger<Web.Services.AnalyticsService>(new LoggerFactory()), ServiceScopeFactoryMock.Object);
        }
    }
}
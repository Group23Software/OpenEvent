using System.Threading.Tasks;
using AutoMapper;
using EntityFrameworkCoreMock;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using OpenEvent.Test.Setups;
using OpenEvent.Web;
using OpenEvent.Web.Contexts;
using OpenEvent.Web.Services;

namespace OpenEvent.Test.Services.EventService
{
    public class EventTestFixture
    {
        protected DbContextMock<ApplicationContext> MockContext;
        protected IMapper Mapper;
        protected IOptions<AppSettings> AppSettings;
        protected IEventService EventService;

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

            EventService = new Web.Services.EventService(MockContext.Object,new Mock<ILogger<Web.Services.EventService>>().Object,Mapper);
        }
    }
}
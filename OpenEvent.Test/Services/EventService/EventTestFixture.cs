using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EntityFrameworkCoreMock;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using NUnit.Framework;
using OpenEvent.Test.Setups;
using OpenEvent.Web;
using OpenEvent.Web.Contexts;
using OpenEvent.Web.Models.Address;
using OpenEvent.Web.Services;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace OpenEvent.Test.Services.EventService
{
    public class EventTestFixture
    {
        protected DbContextMock<ApplicationContext> MockContext;
        protected IMapper Mapper;
        private IOptions<AppSettings> AppSettings;
        protected IEventService EventService;
        protected Mock<HttpMessageHandler> HttpMessageHandlerMock;
        protected HttpClient HttpClientMock;
        protected Mock<IAnalyticsService> AnalyticsServiceMock;
        protected Mock<IRecommendationService> RecommendationServiceMock;
        protected Mock<IDistributedCache> DistributedCacheMock;

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


            DistributedCacheMock = new Mock<IDistributedCache>();

            AnalyticsServiceMock = new Mock<IAnalyticsService>();
            AnalyticsServiceMock.Setup(x => x.CaptureSearch(null, null, new Guid()));
            AnalyticsServiceMock.Setup(x => x.CapturePageView(new Guid(),new Guid()));

            RecommendationServiceMock = new Mock<IRecommendationService>();

            var addressResponse = new SearchAddressResponse()
            {
                Results = new SearchAddressResult[]
                {
                    new() {Position = new CoordinateAbbreviated() {Lat = 0, Lon = 0}}
                }
            };

            var mapResponse = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(addressResponse))
            };
            mapResponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");


            HttpMessageHandlerMock = new Mock<HttpMessageHandler>();
            HttpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()).ReturnsAsync(mapResponse);

            HttpClientMock = new HttpClient(HttpMessageHandlerMock.Object);

            EventService = new Web.Services.EventService(MockContext.Object,
                new Mock<ILogger<Web.Services.EventService>>().Object, Mapper, HttpClientMock, AppSettings,
                AnalyticsServiceMock.Object, RecommendationServiceMock.Object, DistributedCacheMock.Object);
        }
    }
}
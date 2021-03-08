using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using OpenEvent.Web;
using OpenEvent.Web.Contexts;
using OpenEvent.Web.Models.Address;
using OpenEvent.Web.Services;

namespace OpenEvent.Test.Factories
{
    public class EventServiceFactory
    {
        public EventService Create(ApplicationContext context)
        {
            var myProfile = new AutoMapping();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            var mapper = new Mapper(configuration);

            var appSettings = Options.Create(new AppSettings()
            {
                Secret = "this is a secret"
            });

            var distributedCacheMock = new Mock<IDistributedCache>();

            var analyticsServiceMock = new Mock<IAnalyticsService>();
            analyticsServiceMock.Setup(x =>
                x.CaptureSearchAsync(CancellationToken.None, null, null, new Guid(), new DateTime()));
            analyticsServiceMock.Setup(
                x => x.CapturePageViewAsync(CancellationToken.None, new Guid(), new Guid(), new DateTime()));

            var workQueueMock = new Mock<IWorkQueue>();
            workQueueMock.Setup(x => x.QueueWork(new Mock<Func<CancellationToken,Task>>().Object));

            var recommendationServiceMock = new Mock<IRecommendationService>();

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


            var httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()).ReturnsAsync(mapResponse);

            var httpClientMock = new HttpClient(httpMessageHandlerMock.Object);

            return new EventService(
                context,
                new Mock<ILogger<EventService>>().Object,
                mapper,
                httpClientMock,
                appSettings,
                analyticsServiceMock.Object,
                recommendationServiceMock.Object,
                distributedCacheMock.Object, 
                workQueueMock.Object);
        }
    }
}
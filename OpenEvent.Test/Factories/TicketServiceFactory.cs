using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using OpenEvent.Web;
using OpenEvent.Web.Contexts;
using OpenEvent.Web.Services;

namespace OpenEvent.Test.Factories
{
    public class TicketServiceFactory
    {
        public TicketService Create(ApplicationContext context)
        {
            var myProfile = new AutoMapping();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            var mapper = new Mapper(configuration);

            var analyticsServiceMock = new Mock<IAnalyticsService>();
            analyticsServiceMock.Setup(x =>
                x.CaptureSearchAsync(CancellationToken.None, null, null, new Guid(), new DateTime()));
            analyticsServiceMock.Setup(
                x => x.CapturePageViewAsync(CancellationToken.None, new Guid(), new Guid(), new DateTime()));

            var workQueueMock = new Mock<IWorkQueue>();
            workQueueMock.Setup(x => x.QueueWork(new Mock<Func<CancellationToken,Task>>().Object));

            var recommendationServiceMock = new Mock<IRecommendationService>();

            return new TicketService(
                context,
                new Mock<ILogger<TicketService>>().Object, 
                mapper, 
                analyticsServiceMock.Object,
                recommendationServiceMock.Object,
                workQueueMock.Object);
        }
    }
}
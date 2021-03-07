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

            return new TicketService(context,
                new Mock<ILogger<TicketService>>().Object, mapper, analyticsServiceMock.Object);
        }
    }
}
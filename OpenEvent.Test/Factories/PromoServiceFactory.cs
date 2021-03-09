using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using OpenEvent.Web;
using OpenEvent.Web.Contexts;
using OpenEvent.Web.Services;

namespace OpenEvent.Test.Factories
{
    public class PromoServiceFactory : IServiceFactory<PromoService>
    {
        public PromoService Create(ApplicationContext context)
        {
            var myProfile = new AutoMapping();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            var mapper = new Mapper(configuration);

            return new PromoService(new Mock<ILogger<PromoService>>().Object, context, mapper);
        }
    }
}
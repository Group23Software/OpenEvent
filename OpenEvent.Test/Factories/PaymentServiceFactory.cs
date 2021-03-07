using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenEvent.Web;
using OpenEvent.Web.Contexts;
using OpenEvent.Web.Services;

namespace OpenEvent.Test.Factories
{
    public class PaymentServiceFactory
    {
        public PaymentService Create(ApplicationContext context)
        {
            var myProfile = new AutoMapping();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            var mapper = new Mapper(configuration);

            var appSettings = Options.Create(new AppSettings()
            {
                Secret = "this is a secret",
                StripeApiKey =
                    "sk_test_51ILW9dK2ugLXrgQXeYfqg8i0QGAgLXndihLXovHgu47adBimPAedvIwzfr95uffR9TiyleGFAPY7hfSI9mhdmYBF00hkxlAQMv"
            });

            return new PaymentService(context,
                new Logger<Web.Services.PaymentService>(new LoggerFactory()),
                mapper, appSettings);
        }
    }
}
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenEvent.Web;
using OpenEvent.Web.Contexts;
using OpenEvent.Web.Services;

namespace OpenEvent.Test.Factories
{
    public class BankingServiceFactory
    {
        public BankingService Create(ApplicationContext context)
        {
            var myProfile = new AutoMapping();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            var mapper = new Mapper(configuration);

            // Context = new TestDbSetup().Setup();

            var appSettings = Options.Create(new AppSettings()
            {
                Secret = "this is a secret",
                StripeApiKey =
                    "sk_test_51ILW9dK2ugLXrgQXeYfqg8i0QGAgLXndihLXovHgu47adBimPAedvIwzfr95uffR9TiyleGFAPY7hfSI9mhdmYBF00hkxlAQMv"
            });

            return new BankingService(context,
                new Logger<Web.Services.BankingService>(new LoggerFactory()),
                mapper, appSettings);
        }
    }
}
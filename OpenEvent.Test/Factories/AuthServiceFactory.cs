using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenEvent.Web;
using OpenEvent.Web.Contexts;
using OpenEvent.Web.Services;

namespace OpenEvent.Test.Factories
{
    public class AuthServiceFactory
    {
        public AuthService Create(ApplicationContext context)
        {
            var myProfile = new AutoMapping();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            var mapper = new Mapper(configuration);

            // Context = new TestDbSetup().Setup();

            var appSettings = Options.Create(new AppSettings()
            {
                Secret = "this is a secret"
            });

            return new AuthService(context,
                new Logger<Web.Services.AuthService>(new LoggerFactory()), appSettings, mapper);
        }
    }
}
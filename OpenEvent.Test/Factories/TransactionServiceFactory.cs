using System;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using OpenEvent.Web;
using OpenEvent.Web.Contexts;
using OpenEvent.Web.Services;

namespace OpenEvent.Test.Factories
{
    public class TransactionServiceFactory
    {
        public TransactionService Create(ApplicationContext context)
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

            Mock<IServiceProvider> serviceProvider = new Mock<IServiceProvider>();
            serviceProvider.Setup(x => x.GetService(typeof(ApplicationContext))).Returns(context);

            Mock<IServiceScope> serviceScopeMock = new Mock<IServiceScope>();
            serviceScopeMock.Setup(x => x.ServiceProvider).Returns(() => serviceProvider.Object);

            var serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();
            serviceScopeFactoryMock.Setup(x => x.CreateScope()).Returns(() => serviceScopeMock.Object);

            var emailServiceMock = new Mock<IEmailService>();

            return new TransactionService(
                context,
                new Logger<TransactionService>(new LoggerFactory()),
                mapper,
                appSettings,
                serviceScopeFactoryMock.Object,
                emailServiceMock.Object
            );
        }
    }
}
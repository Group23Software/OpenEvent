using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using OpenEvent.Web.Contexts;
using OpenEvent.Web.Services;

namespace OpenEvent.Test.Factories
{
    // public class AnalyticsServiceFactory
    // {
    //     public AnalyticsService Create(ApplicationContext context)
    //     {
    //         var recommendationServiceMock = new Mock<IRecommendationService>();
    //         
    //         var s = new scopeFactory();
    //
    //
    //         return new AnalyticsService(
    //             new Logger<AnalyticsService>(new LoggerFactory()),
    //             s,
    //             recommendationServiceMock.Object);
    //     }
    //
    //
    //     public class scopeFactory : IServiceScopeFactory
    //     {
    //         public IServiceScope CreateScope()
    //         {
    //             return new serviceScope();
    //         }
    //     }
    //     
    //     public class serviceScope: IServiceScope
    //     {
    //         public serviceScope()
    //         {
    //             this.ServiceProvider = new serviceProvider();
    //         }
    //         
    //         public void Dispose()
    //         {
    //             throw new NotImplementedException();
    //         }
    //
    //         public IServiceProvider ServiceProvider { get; }
    //     }
    //
    //     public class serviceProvider : IServiceProvider
    //     {
    //         public object? GetService(Type serviceType)
    //         {
    //             return new AnalyticsService();
    //         }
    //     }
    // }
}
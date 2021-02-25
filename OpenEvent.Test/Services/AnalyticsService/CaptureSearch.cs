using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace OpenEvent.Test.Services.AnalyticsService
{
    [TestFixture]
    public class CaptureSearch : AnalyticsTestFixture
    {
        [Test]
        public async Task Should_Capture()
        {
            AnalyticsService.CaptureSearch("","",new Guid("046E876E-D413-45AF-AC2A-552D7AA46C5C"));
            // await Task.Delay(1);
            // var pageViewEvents =  MockContext.Object.PageViewEvents.ToList();
            
        }
    }
}
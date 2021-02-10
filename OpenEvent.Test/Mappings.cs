using AutoMapper;
using NUnit.Framework;
using OpenEvent.Web;

namespace OpenEvent.Test
{
    [TestFixture]
    public class Mappings
    {
        [Test]
        public void Map_Should_HaveValidConfig()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapping>());
            config.AssertConfigurationIsValid();
        }
    }
}
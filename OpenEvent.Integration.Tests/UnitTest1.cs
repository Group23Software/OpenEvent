using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using OpenEvent.Web;

namespace OpenEvent.Integration.Tests
{
    [TestFixture]
    public class Tests
    {
        private ApplicationFactory<TestStartup> Factory;
        private HttpClient Client;
        
        [SetUp]
        public void Setup()
        {
            Factory = new ApplicationFactory<TestStartup>();
            Client = Factory.CreateClient();
        }

        [Test]
        public async Task Test1()
        {
            var response = await Client.GetAsync("/");

            response.StatusCode.Should().Be(404);
            
            // Assert.IsTrue(true);
        }
    }
}
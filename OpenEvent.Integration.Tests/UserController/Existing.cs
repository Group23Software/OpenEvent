using System;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace OpenEvent.Integration.Tests.UserController
{
    [TestFixture]
    public class Existing
    {
        private HttpClient Client;

        [SetUp]
        public void Setup()
        {
            Client = ClientFactory.Create();
        }


        [Test]
        public async Task Should_Check_Email_Exists()
        {
            var builder = new UriBuilder(TestData.BaseUrl + "/api/user/EmailExists") {Query = "email=test@test.co.uk"};
            var response = await Client.GetAsync(builder.Uri);
            response.StatusCode.Should().Be(200);
        }

        [Test]
        public async Task Should_Check_Phone_Exists()
        {
            var builder = new UriBuilder(TestData.BaseUrl + "/api/user/PhoneExists") {Query = "phone=+4407852276048"};
            var response = await Client.GetAsync(builder.Uri);
            response.StatusCode.Should().Be(200);
        }

        [Test]
        public async Task Should_Check_Username_Exists()
        {
            var builder = new UriBuilder(TestData.BaseUrl + "/api/user/UserNameExists") {Query = "username=test"};
            var response = await Client.GetAsync(builder.Uri);
            response.StatusCode.Should().Be(200);
        }
    }
}
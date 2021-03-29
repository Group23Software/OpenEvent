using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using OpenEvent.Data.Models.Auth;

namespace OpenEvent.Integration.Tests.AuthController
{
    [TestFixture]
    public class Login
    {
        private HttpClient Client;

        [SetUp]
        public void Setup()
        {
            Client = ClientFactory.Create();
        }

        [Test]
        public async Task Should_Login()
        {
            LoginBody body = new()
            {
                Email = "exists@email.co.uk",
                Password = "Password",
                Remember = false
            };

            var response = await Client.PostAsJsonAsync(new UriBuilder(TestData.BaseUrl + "/api/auth/login").Uri, body);
            response.StatusCode.Should().Be(200);
        }
    }
}
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
    public class Authenticate
    {
        private HttpClient Client;

        [SetUp]
        public void Setup()
        {
            Client = ClientFactory.Create();
        }

        [Test]
        public async Task Should_Auth()
        {
            var loggedUser = await TestData.LogUserIn(Client);

            AuthBody body = new()
            {
                Id = loggedUser.Id
            };

            var response = await Client.PostAsJsonAsync(new UriBuilder(TestData.BaseUrl + "/api/auth/authenticateToken").Uri,body);
            response.StatusCode.Should().Be(200);
        }
    }
}
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using OpenEvent.Web.Models.User;

namespace OpenEvent.Integration.Tests.UserController
{
    [TestFixture]
    public class Create
    {
        private HttpClient Client;

        [SetUp]
        public void Setup()
        {
            Client = ClientFactory.Create();
        }

        [Test]
        public async Task Should_Create_User()
        {
            var loggedUser = await TestData.LogUserIn(Client);
            
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loggedUser.Token);
            
            var builder = new UriBuilder(TestData.BaseUrl + "/api/user");

            NewUserBody body = new NewUserBody()
            {
                UserName = "TestUsername",
                Email = "test@test.co.uk",
                Avatar = {},
                Password = "Password@1",
                FirstName = "Test",
                LastName = "User",
                PhoneNumber = "+4407852276048",
                DateOfBirth = DateTime.Today.AddYears(-18)
            };

            var response = await Client.PostAsJsonAsync(builder.Uri,body);
            response.StatusCode.Should().Be(200);
        }
    }
}
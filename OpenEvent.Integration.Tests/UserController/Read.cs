using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using OpenEvent.Web.Models.User;

namespace OpenEvent.Integration.Tests.UserController
{
    [TestFixture]
    public class Read
    {
        private HttpClient Client;

        [SetUp]
        public void Setup()
        {
            Client = ClientFactory.Create();
        }

        [Test]
        public async Task Should_Get_User_For_Account_Page()
        {
            var loggedUser = await TestData.LogUserIn(Client);
            
            var builder = new UriBuilder(TestData.BaseUrl + "/api/user/Account") {Query = $"id={loggedUser.Id}"};

            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",loggedUser.Token);
            
            var response = await Client.GetAsync(builder.Uri);
            response.StatusCode.Should().Be(200);
            
            var user = await response.Content.ReadFromJsonAsync<UserAccountModel>();
            user.Should().NotBeNull();
            user.Should().BeOfType<UserAccountModel>();
        }
    }
}
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace OpenEvent.Integration.Tests.UserController
{
    [TestFixture]
    public class Destroy
    {
        private HttpClient Client;

        [SetUp]
        public void Setup()
        {
            Client = ClientFactory.Create();
        }

        // TODO: user destroy does not work because of FK constraints
        // [Test]
        // public async Task Should_Destroy_User()
        // {
        //     var loggedUser = await TestData.LogUserIn(Client);
        //
        //     var builder = new UriBuilder(TestData.BaseUrl + "/api/user/") {Query = $"id={loggedUser.Id}"};
        //     
        //     Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",loggedUser.Token);
        //     
        //     var response = await Client.DeleteAsync(builder.Uri);
        //     response.StatusCode.Should().Be(200);
        // }
    }
}
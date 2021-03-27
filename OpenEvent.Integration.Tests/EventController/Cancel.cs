using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace OpenEvent.Integration.Tests.EventController
{
    [TestFixture]
    public class Cancel
    {
        private HttpClient Client;

        [SetUp]
        public void Setup()
        {
            Client = ClientFactory.Create();
        }

        [Test]
        public async Task Should_Cancel_Event()
        {
            var loggedUser = await TestData.LogUserIn(Client);
            
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loggedUser.Token);
            Client.DefaultRequestHeaders.Add("EventId", TestData.Event.Id.ToString());
            Client.DefaultRequestHeaders.Add("UserId", loggedUser.Id.ToString());
            
            var u = new UriBuilder(TestData.BaseUrl + "/api/event/cancel") {Query = $"id={TestData.Event.Id}"}.Uri;
            var response = await Client.PostAsync(u,null);
            response.StatusCode.Should().Be(200);
        }
        
        [Test]
        public async Task Should_Not_Allow()
        {
            var u = new UriBuilder(TestData.BaseUrl + "/api/event/cancel") {Query = $"id={TestData.Event.Id}"}.Uri;
            var response = await Client.PostAsync(u,null);
            response.StatusCode.Should().Be(401);
        }
    }
}
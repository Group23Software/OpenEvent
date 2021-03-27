using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;
using OpenEvent.Web.Models.Event;

namespace OpenEvent.Integration.Tests.EventController
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
        public async Task Should_Get_Event_For_Pubic()
        {
            var response = await Client.GetAsync(new UriBuilder(TestData.BaseUrl + "/api/event/public") {Query = $"id={TestData.Event.Id}"}.Uri);
            response.StatusCode.Should().Be(200);
            // var e = await response.Content.ReadFromJsonAsync<EventDetailModel>();
            // e.Should().NotBeNull();
            // e.Description.Should().BeEquivalentTo(TestData.Event.Description);
        }

        [Test]
        public async Task Should_Get_Event_For_Host()
        {
            var loggedUser = await TestData.LogUserIn(Client);

            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loggedUser.Token);
            Client.DefaultRequestHeaders.Add("EventId", TestData.Event.Id.ToString());
            Client.DefaultRequestHeaders.Add("UserId", loggedUser.Id.ToString());

            var response = await Client.GetAsync(new UriBuilder(TestData.BaseUrl + "/api/event/forHost") {Query = $"id={TestData.Event.Id}"}.Uri);
            response.StatusCode.Should().Be(200);

            var e = await response.Content.ReadAsStringAsync();
        }

        [Test]
        public async Task Should_Not_Allow_Non_Owner()
        {
            var response = await Client.GetAsync(new UriBuilder(TestData.BaseUrl + "/api/event/forHost") {Query = $"id={TestData.Event.Id}"}.Uri);
            response.StatusCode.Should().Be(401);
        }

        [Test]
        public async Task Should_Get_All_Users_Events()
        {
            var loggedUser = await TestData.LogUserIn(Client);
            
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loggedUser.Token);

            var response = await Client.GetAsync(new UriBuilder(TestData.BaseUrl + "/api/event/host") {Query = $"id={loggedUser.Id}"}.Uri);
            response.StatusCode.Should().Be(200);

            // var events = JsonConvert.DeserializeObject<List<EventHostModel>>(await response.Content.ReadAsStringAsync());
            // events.Should().NotContainNulls();
        }
    }
}
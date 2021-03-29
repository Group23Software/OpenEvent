using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using OpenEvent.Data.Models.Event;

namespace OpenEvent.Integration.Tests.EventController
{
    [TestFixture]
    public class Update
    {
        private HttpClient Client;

        [SetUp]
        public void Setup()
        {
            Client = ClientFactory.Create();
        }

        [Test]
        public async Task Should_Update_Event()
        {
            var loggedUser = await TestData.LogUserIn(Client);

            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loggedUser.Token);
            Client.DefaultRequestHeaders.Add("EventId", TestData.Event.Id.ToString());
            Client.DefaultRequestHeaders.Add("UserId", loggedUser.Id.ToString());
            
            UpdateEventBody body = new()
            {
                Id = TestData.Event.Id,
                Address = TestData.Event.Address,
                Description = TestData.Event.Description,
                Finished = TestData.Event.Finished,
                Name = "Updated event",
                Price = TestData.Event.Price,
                EndLocal = TestData.Event.EndLocal,
                StartLocal = TestData.Event.StartLocal,
                IsOnline = TestData.Event.IsOnline,
            };

            var response = await Client.PostAsJsonAsync(new UriBuilder(TestData.BaseUrl + "/api/event/update").Uri, body);
            response.StatusCode.Should().Be(200);
        }
    }
}
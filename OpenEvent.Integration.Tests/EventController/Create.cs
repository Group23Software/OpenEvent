using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using OpenEvent.Web.Models.Address;
using OpenEvent.Web.Models.Category;
using OpenEvent.Web.Models.Event;

namespace OpenEvent.Integration.Tests.EventController
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
        public async Task Should_Create_Event()
        {
            var loggedUser = await TestData.LogUserIn(Client);
            
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loggedUser.Token);
            
            CreateEventBody body = new()
            {
                Name = "Event Name",
                Address = null,
                Description = "Description",
                Price = 1000,
                EndLocal = DateTime.Now,
                StartLocal = DateTime.Now,
                HostId = loggedUser.Id,
                IsOnline = true,
                SocialLinks = new List<SocialLinkViewModel>(),
                Images = new List<ImageViewModel>(),
                NumberOfTickets = 10,
                Thumbnail = new ImageViewModel(),
                Categories = new List<Category>(){TestData.Categories[0]}
            };
            
            var response = await Client.PostAsJsonAsync(TestData.BaseUrl + "/api/event/", body);
            response.StatusCode.Should().Be(200);
        }
    }
}
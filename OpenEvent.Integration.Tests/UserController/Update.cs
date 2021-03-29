using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using OpenEvent.Data.Models.Address;
using OpenEvent.Data.Models.User;

namespace OpenEvent.Integration.Tests.UserController
{
    [TestFixture]
    public class Update
    {
        private class updatedUsername
        {
            public string Username { get; set; }
        }
        
        private class updatedAavatar
        {
            public string Avatar { get; set; }
        }
        
        private class updatedAddress
        {
            public Address Address { get; set; }
        }

        private class updatedThemePref
        {
            public bool isDarkMode { get; set; }
        }

        private HttpClient Client;
        private UserViewModel LoggedUser;

        [SetUp]
        public async Task Setup()
        {
            Client = ClientFactory.Create();
            LoggedUser = await TestData.LogUserIn(Client);
        }

        [Test]
        public async Task Should_Update_User_Name()
        {

            UpdateUserNameBody body = new()
            {
                Id = LoggedUser.Id,
                UserName = "Updated username"
            };

            var response = await Client.PostAsJsonAsync(new UriBuilder(TestData.BaseUrl + "/api/user/updateUserName").Uri, body);

            response.StatusCode.Should().Be(200);

            var username = await response.Content.ReadFromJsonAsync<updatedUsername>();

            username.Username.Should().BeEquivalentTo(body.UserName);
        }

        [Test]
        public async Task Should_Update_Avatar()
        {

            UpdateAvatarBody body = new()
            {
                Id = LoggedUser.Id,
                Avatar = new byte[] {1, 1, 1, 1, 1, 1}
            };

            var response = await Client.PostAsJsonAsync(new UriBuilder(TestData.BaseUrl + "/api/user/updateAvatar").Uri, body);

            response.StatusCode.Should().Be(200);

            var avatar = await response.Content.ReadFromJsonAsync<updatedAavatar>();

            avatar.Avatar.Should().BeEquivalentTo(Encoding.UTF8.GetString(body.Avatar));
        }

        [Test]
        public async Task Should_Update_Address()
        {

            UpdateUserAddressBody body = new()
            {
                Id = LoggedUser.Id,
                Address = new Address()
                {
                    AddressLine1 = "AddressLine1",
                    AddressLine2 = "AddressLine2",
                    City = "City",
                    CountryCode = "CC",
                    CountryName = "CountryName",
                    PostalCode = "AA1 1AA"
                }
            };

            var response = await Client.PostAsJsonAsync(new UriBuilder(TestData.BaseUrl + "/api/user/updateAddress").Uri, body);

            response.StatusCode.Should().Be(200);

            var address = await response.Content.ReadFromJsonAsync<updatedAddress>();

            address.Address.Should().BeEquivalentTo(body.Address);
        }
        
        [Test]
        public async Task Should_Update_Theme_Pref()
        {

            UpdateThemePreferenceBody body = new()
            {
                Id = LoggedUser.Id,
                IsDarkMode = true
            };

            var response = await Client.PostAsJsonAsync(new UriBuilder(TestData.BaseUrl + "/api/user/updateThemePreference").Uri, body);

            response.StatusCode.Should().Be(200);

            var themePref = await response.Content.ReadFromJsonAsync<updatedThemePref>();

            themePref.isDarkMode.Should().Be(body.IsDarkMode);
        }
    }
}
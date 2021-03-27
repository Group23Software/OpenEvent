using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using OpenEvent.Web.Exceptions;
using OpenEvent.Web.Models.User;

namespace OpenEvent.Test.Controllers.UserController
{
    [TestFixture]
    public class UpdateThemePreference
    {
        private readonly Mock<Web.Services.IUserService> UserServiceMock = new();

        private Web.Controllers.UserController UserController;

        private readonly UpdateThemePreferenceBody UpdateThemePreferenceBody = new()
        {
            Id = new Guid("A4EF213D-7B3E-4AB8-A396-690201BC8BFA"),
            IsDarkMode = true
        };

        [SetUp]
        public async Task Setup()
        {
            UserServiceMock.Setup(x => x.UpdateThemePreference(UpdateThemePreferenceBody.Id, UpdateThemePreferenceBody.IsDarkMode))
                .ReturnsAsync(UpdateThemePreferenceBody.IsDarkMode);
            UserServiceMock.Setup(x => x.UpdateThemePreference(new Guid(), false)).ThrowsAsync(new UserNotFoundException());
            
            UserController = new Web.Controllers.UserController(UserServiceMock.Object,
                new Mock<ILogger<Web.Controllers.UserController>>().Object);
        }

        [Test]
        public async Task ShouldUpdateThemePreference()
        {
            var result = await UserController.UpdateThemePreference(UpdateThemePreferenceBody);
            result.Should().BeOfType<OkObjectResult>().Subject.Value.Should().NotBeNull();
        }
        
        [Test]
        public async Task ShouldNotFindUser()
        {
            var result = await UserController.UpdateThemePreference(new UpdateThemePreferenceBody(){Id = new Guid()});
            result.Should().BeOfType<BadRequestObjectResult>().Subject.Value.Should().BeOfType<UserNotFoundException>();
        }
    }
}
using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using OpenEvent.Web.Exceptions;
using OpenEvent.Web.Models.Auth;
using OpenEvent.Web.Models.User;

namespace OpenEvent.Test.Controllers.AuthController
{
    [TestFixture]
    public class Authenticate
    {
        private readonly Mock<Web.Services.IAuthService> AuthServiceMock = new();

        private Web.Controllers.AuthController AuthController;

        private readonly UserViewModel UserViewModel = new UserViewModel()
        {
            Id = new Guid("07279DE6-74E9-4CA0-AC4A-D78750016082"),
            UserName = "Test User",
            Avatar = "0000",
            Token = "Token",
            IsDarkMode = true
        };

        [SetUp]
        public async Task Setup()
        {
            AuthServiceMock.Setup(x => x.Authenticate(UserViewModel.Id)).ReturnsAsync(UserViewModel);
            AuthServiceMock.Setup(x => x.Authenticate(new Guid())).ThrowsAsync(new UserNotFoundException());
            AuthController = new Web.Controllers.AuthController(AuthServiceMock.Object,
                new Mock<ILogger<Web.Controllers.AuthController>>().Object);
        }

        [Test]
        public async Task ShouldAuthenticate()
        {
            var result = await AuthController.Authenticate(new AuthBody {Id = UserViewModel.Id});
            result.Result.Should().BeOfType<OkObjectResult>().Subject.Value.Should().BeOfType<UserViewModel>();
        }

        [Test]
        public async Task ShouldNotAuthenticate()
        {
            var result = await AuthController.Authenticate(new AuthBody {Id = new Guid()});
            result.Result.Should().BeOfType<UnauthorizedObjectResult>().Subject.Value.Should().Be("User Not Found");
        }
    }
}
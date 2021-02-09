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
    public class Login
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

        private readonly LoginBody LoginBody = new LoginBody()
        {
            Email = "email@email.co.uk",
            Password = "Password",
            Remember = true
        };

        private readonly LoginBody EmptyLoginBody = new LoginBody()
        {
            Email = "", 
            Password = "", 
            Remember = false
        };

        [SetUp]
        public async Task Setup()
        {
            AuthServiceMock.Setup(x => x.Login(LoginBody.Email, LoginBody.Password, LoginBody.Remember))
                .ReturnsAsync(UserViewModel);
            AuthServiceMock.Setup(x => x.Login(EmptyLoginBody.Email,EmptyLoginBody.Password,EmptyLoginBody.Remember)).ThrowsAsync(new EventNotFoundException());
            AuthController = new Web.Controllers.AuthController(AuthServiceMock.Object,
                new Mock<ILogger<Web.Controllers.AuthController>>().Object);
        }

        [Test]
        public async Task ShouldLogin()
        {
            var result = await AuthController.Login(LoginBody);
            result.Result.Should().BeOfType<OkObjectResult>().Subject.Value.Should().BeOfType<UserViewModel>();
        }

        [Test]
        public async Task ShouldNotFindUser()
        {
            var result = await AuthController.Login(EmptyLoginBody);
            result.Result.Should()
                .BeOfType<UnauthorizedObjectResult>()
                .Subject.Value.Should().Be("Event Not Found");
        }
    }
}
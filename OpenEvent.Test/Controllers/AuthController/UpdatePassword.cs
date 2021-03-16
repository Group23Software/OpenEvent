using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using OpenEvent.Web.Exceptions;
using OpenEvent.Web.Models.Auth;

namespace OpenEvent.Test.Controllers.AuthController
{
    [TestFixture]
    public class UpdatePassword
    {
        private readonly Mock<Web.Services.IAuthService> AuthServiceMock = new();

        private Web.Controllers.AuthController AuthController;

        private readonly UpdatePasswordBody UpdatePasswordBody = new()
        {
            Id = new Guid("046E876E-D413-45AF-AC2A-552D7AA46C5C"),
            Password = "New Password"
        };

        [SetUp]
        public async Task Setup()
        {
            AuthServiceMock.Setup(x => x.UpdatePassword(UpdatePasswordBody.Id, UpdatePasswordBody.Password));
            AuthServiceMock.Setup(x => x.UpdatePassword(Guid.Empty, null)).ThrowsAsync(new UserNotFoundException());

            AuthController = new Web.Controllers.AuthController(AuthServiceMock.Object,
                new Mock<ILogger<Web.Controllers.AuthController>>().Object);
        }

        [Test]
        public async Task ShouldUpdatePassword()
        {
            var result = await AuthController.UpdatePassword(UpdatePasswordBody);
            result.Should().BeOfType<OkResult>();
        }

        [Test]
        public async Task ShouldNotFindUser()
        {
            var result =
                await AuthController.UpdatePassword(new UpdatePasswordBody() {Id = Guid.Empty, Password = null});
            result.Should().BeOfType<BadRequestObjectResult>().Subject.Value.Should().Be("User Not Found");
        }
    }
}
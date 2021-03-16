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
    public class Create
    {
        private readonly Mock<Web.Services.IUserService> UserServiceMock = new();

        private Web.Controllers.UserController UserController;

        private readonly NewUserInput ExistingUserInput = new() {Email = "existing@email.co.uk"};

        private readonly NewUserInput NewUserInput = new()
        {
            UserName = "Username"
        };

        private readonly UserViewModel UserViewModel = new()
        {
            UserName = "Username"
        };

        [SetUp]
        public async Task Setup()
        {
            UserServiceMock.Setup(x => x.Create(NewUserInput));
            UserServiceMock.Setup(x => x.Create(ExistingUserInput)).ThrowsAsync(new UserAlreadyExistsException());

            UserController = new Web.Controllers.UserController(UserServiceMock.Object,
                new Mock<ILogger<Web.Controllers.UserController>>().Object);
        }

        [Test]
        public async Task ShouldCreate()
        {
            var result = await UserController.Create(NewUserInput);
            result.Should().BeOfType<OkResult>();
        }

        [Test]
        public async Task UserShouldAlreadyExist()
        {
            var result = await UserController.Create(ExistingUserInput);
            result.Should().BeOfType<BadRequestObjectResult>().Subject.Value.Should().BeOfType<UserAlreadyExistsException>();
        }
    }
}
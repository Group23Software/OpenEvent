using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace OpenEvent.Test.Controllers.UserController
{
    [TestFixture]
    public class EmailExists
    {
        private readonly Mock<Web.Services.IUserService> UserServiceMock = new();

        private Web.Controllers.UserController UserController;

        [SetUp]
        public async Task Setup()
        {
            UserServiceMock.Setup(x => x.EmailExists("ExistingEmail")).ReturnsAsync(true);
            UserServiceMock.Setup(x => x.EmailExists("NewEmail")).ReturnsAsync(false);
            UserController = new Web.Controllers.UserController(UserServiceMock.Object,
                new Mock<ILogger<Web.Controllers.UserController>>().Object);
        }

        [Test]
        public async Task ShouldNotExist()
        {
            var result = await UserController.EmailExists("NewEmail");
            result.Result.Should().BeOfType<OkObjectResult>().Subject.Value.Should().Be(false);
        }
        
        [Test]
        public async Task ShouldExist()
        {
            var result = await UserController.EmailExists("ExistingEmail");
            result.Result.Should().BeOfType<OkObjectResult>().Subject.Value.Should().Be(true);
        }
    }
}
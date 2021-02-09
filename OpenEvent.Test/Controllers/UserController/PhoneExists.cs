using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace OpenEvent.Test.Controllers.UserController
{
    [TestFixture]
    public class PhoneExists
    {
        private readonly Mock<Web.Services.IUserService> UserServiceMock = new();

        private Web.Controllers.UserController UserController;

        [SetUp]
        public async Task Setup()
        {
            UserServiceMock.Setup(x => x.PhoneExists("ExistingPhoneNumber")).ReturnsAsync(true);
            UserServiceMock.Setup(x => x.PhoneExists("NewPhoneNumber")).ReturnsAsync(false);
            UserController = new Web.Controllers.UserController(UserServiceMock.Object,
                new Mock<ILogger<Web.Controllers.UserController>>().Object);
        }

        [Test]
        public async Task ShouldNotExist()
        {
            var result = await UserController.PhoneExists("NewPhoneNumber");
            result.Result.Should().BeOfType<OkObjectResult>().Subject.Value.Should().Be(false);
        }
        
        [Test]
        public async Task ShouldExist()
        {
            var result = await UserController.PhoneExists("ExistingPhoneNumber");
            result.Result.Should().BeOfType<OkObjectResult>().Subject.Value.Should().Be(true);
        }
    }
}
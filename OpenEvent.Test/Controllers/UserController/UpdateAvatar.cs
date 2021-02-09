using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using OpenEvent.Web.Exceptions;
using OpenEvent.Web.Models.Event;
using OpenEvent.Web.Models.User;

namespace OpenEvent.Test.Controllers.UserController
{
    [TestFixture]
    public class UpdateAvatar
    {
        private readonly Mock<Web.Services.IUserService> UserServiceMock = new();

        private Web.Controllers.UserController UserController;

        private readonly UpdateAvatarBody UpdateAvatarBody = new()
        {
            Id = new Guid("A4EF213D-7B3E-4AB8-A396-690201BC8BFA"),
            Avatar = new Byte[]{1,0,1,0}
        };

        [SetUp]
        public async Task Setup()
        {
            UserServiceMock.Setup(x => x.UpdateAvatar(UpdateAvatarBody.Id, UpdateAvatarBody.Avatar))
                .ReturnsAsync("Avatar as string");
            UserServiceMock.Setup(x => x.UpdateAvatar(new Guid(), null)).ThrowsAsync(new UserNotFoundException());
            
            UserController = new Web.Controllers.UserController(UserServiceMock.Object,
                new Mock<ILogger<Web.Controllers.UserController>>().Object);
        }

        [Test]
        public async Task ShouldUpdateAvatar()
        {
            var result = await UserController.UpdateAvatar(UpdateAvatarBody);
            result.Result.Should().BeOfType<OkObjectResult>().Subject.Value.Should().NotBeNull();
        }
        
        [Test]
        public async Task ShouldNotFindUser()
        {
            var result = await UserController.UpdateAvatar(new UpdateAvatarBody(){Id = new Guid()});
            result.Result.Should().BeOfType<BadRequestObjectResult>().Subject.Value.Should().BeOfType<UserNotFoundException>();
        }
    }
}
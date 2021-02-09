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
    public class UpdateUserName
    {
        private readonly Mock<Web.Services.IUserService> UserServiceMock = new();

        private Web.Controllers.UserController UserController;

        private readonly UpdateUserNameBody UpdateUserNameBody = new()
        {
            Id = new Guid("A4EF213D-7B3E-4AB8-A396-690201BC8BFA"),
            UserName = "Updated Username"
        };

        [SetUp]
        public async Task Setup()
        {
            UserServiceMock.Setup(x => x.UpdateUserName(UpdateUserNameBody.Id, UpdateUserNameBody.UserName))
                .ReturnsAsync(UpdateUserNameBody.UserName);
            UserServiceMock.Setup(x => x.UpdateUserName(new Guid(), null)).ThrowsAsync(new UserNotFoundException());
            
            UserController = new Web.Controllers.UserController(UserServiceMock.Object,
                new Mock<ILogger<Web.Controllers.UserController>>().Object);
        }

        [Test]
        public async Task ShouldUpdateUserName()
        {
            var result = await UserController.UpdateUserName(UpdateUserNameBody);
            result.Result.Should().BeOfType<OkObjectResult>().Subject.Value.Should().NotBeNull();
        }
        
        [Test]
        public async Task ShouldNotFindUser()
        {
            var result = await UserController.UpdateUserName(new UpdateUserNameBody(){Id = new Guid()});
            result.Result.Should().BeOfType<BadRequestObjectResult>().Subject.Value.Should().BeOfType<UserNotFoundException>();
        }
    }
}
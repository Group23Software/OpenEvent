using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using OpenEvent.Data.Models.User;
using OpenEvent.Web.Exceptions;

namespace OpenEvent.Test.Controllers.UserController
{
    [TestFixture]
    public class GetAccountUser
    {
        private readonly Mock<Web.Services.IUserService> UserServiceMock = new();

        private Web.Controllers.UserController UserController;

        private readonly UserAccountModel UserAccountModel = new UserAccountModel()
        {
            Id = new Guid("7EBBEBDF-9228-4008-8C76-6B5AE6C525EE"),
        };

        [SetUp]
        public async Task Setup()
        {
            UserServiceMock.Setup(x => x.Get(UserAccountModel.Id)).ReturnsAsync(UserAccountModel);
            UserServiceMock.Setup(x => x.Get(new Guid())).ThrowsAsync(new UserNotFoundException());
            
            UserController = new Web.Controllers.UserController(UserServiceMock.Object,
                new Mock<ILogger<Web.Controllers.UserController>>().Object);
        }

        [Test]
        public async Task ShouldGet()
        {
            var result = await UserController.GetAccountUser(UserAccountModel.Id);
            result.Should().BeOfType<ActionResult<UserAccountModel>>();
        }

        [Test]
        public async Task ShouldNotFindUser()
        {
            var result = await UserController.GetAccountUser(new Guid());
            result.Result.Should().BeOfType<BadRequestObjectResult>().Subject.Value.Should().BeOfType<UserNotFoundException>();
        }
    }
}
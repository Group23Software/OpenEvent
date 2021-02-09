using System;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using OpenEvent.Web.Exceptions;

namespace OpenEvent.Test.Controllers.UserController
{
    [TestFixture]
    public class Destroy
    {
        private readonly Mock<Web.Services.IUserService> UserServiceMock = new();

        private Web.Controllers.UserController UserController;

        private readonly Guid UserId = new Guid("7EBBEBDF-9228-4008-8C76-6B5AE6C525EE");

        [SetUp]
        public async Task Setup()
        {
            UserServiceMock.Setup(x => x.Destroy(UserId));
            UserServiceMock.Setup(x => x.Destroy(new Guid())).ThrowsAsync(new UserNotFoundException());
            UserController = new Web.Controllers.UserController(UserServiceMock.Object,
                new Mock<ILogger<Web.Controllers.UserController>>().Object);
        }

        [Test]
        public async Task ShouldDestroyUser()
        {
            var result = await UserController.Destroy(UserId);
            result.Should().BeOfType<OkResult>();
        }
        
        [Test]
        public async Task ShouldNotFindUser()
        {
            var result = await UserController.Destroy(new Guid());
            result.Should().BeOfType<BadRequestObjectResult>().Subject.Value.Should().BeOfType<UserNotFoundException>();
        }
    }
}
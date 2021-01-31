using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using OpenEvent.Web.Models.User;

namespace OpenEvent.Test.Services.UserService
{
    [TestFixture]
    public class Create : UserTestFixture
    {
        [Test]
        public async Task ShouldCreateNewUser()
        {
            NewUserInput user = new NewUserInput()
            {
                Email = "email@email.co.uk",
                Password = "Password",
                FirstName = "Joe",
                LastName = "Blogs",
                UserName = "JoeBlogs",
                PhoneNumber = "0000000000",
                Avatar = new byte[100],
                Remember = false,
                DateOfBirth = DateTime.Now.AddYears(-18)
            };

            var result = await UserService.Create(user);
            result.Should().NotBeNull();
        }

        [Test]
        public async Task ShouldFailNonUniqueUser()
        {
            NewUserInput user = new NewUserInput()
            {
                Email = "exists@email.co.uk",
                Password = "Fail",
                UserName = "ExistingUser"
            };

            FluentActions.Invoking(async () => await UserService.Create(user))
                .Should().Throw<Exception>()
                .WithMessage("User already exists");
        }

        [Test]
        public async Task ShouldThrowDbUpdateException()
        {
            MockContext.Setup(c => c.SaveChangesAsync(new CancellationToken()))
                .ReturnsAsync(() => throw new DbUpdateException());


            NewUserInput user = new NewUserInput()
            {
                Email = "email@email.co.uk",
                Password = "Password",
                FirstName = "Joe",
                LastName = "Blogs",
                UserName = "JoeBlogs",
                PhoneNumber = "0000000000",
                Avatar = new byte[100],
                Remember = false,
                DateOfBirth = DateTime.Now.AddYears(-18)
            };


            FluentActions.Invoking(async () => await UserService.Create(user)).Should().Throw<DbUpdateException>();
        }
    }
}
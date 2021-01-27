using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using OpenEvent.Web.Models;

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
    }
}
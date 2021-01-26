using System;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using OpenEvent.Test.Setups;
using OpenEvent.Web;
using OpenEvent.Web.Contexts;
using OpenEvent.Web.Models;

namespace OpenEvent.Test.Services.UserService
{
    [TestFixture]
    public class Create : BasicTestFixture
    {
        [Test]
        public async Task ShouldCreateNewUser()
        {
            var userService =
                new Web.Services.UserService(Context, new Logger<Web.Services.UserService>(new LoggerFactory()));

            NewUserInput user = new NewUserInput()
            {
                Email = "email@email.co.uk",
                Password = "Password",
                FirstName = "Joe",
                LastName = "Blogs",
                UserName = "JoeBlogs",
                PhoneNumber = "0000000000"
            };

            var result = await userService.Create(user);
            result.Should().NotBeNull();
        }
        
        [Test]
        public async Task ShouldFailNonUniqueUser()
        {
            var userService = new Web.Services.UserService(Context, new Logger<Web.Services.UserService>(new LoggerFactory()));

            NewUserInput user = new NewUserInput()
            {
                Email = "exists@email.co.uk",
                Password = "Fail",
                UserName = "ExistingUser"
            };
            
            FluentActions.Invoking(async () => await userService.Create(user))
                .Should().Throw<Exception>()
                .WithMessage("User already exists");
        }
    }
}
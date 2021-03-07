using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using OpenEvent.Test.Factories;
using OpenEvent.Test.Setups;
using OpenEvent.Web.Exceptions;
using OpenEvent.Web.Models.User;

namespace OpenEvent.Test.Services.UserService
{
    [TestFixture]
    public class Create
    {
        [Test]
        public async Task ShouldCreateNewUser()
        {
            await using (var context = new DbContextFactory().CreateContext())
            {
                var service = new UserServiceFactory().Create(context);

                NewUserInput user = new NewUserInput()
                {
                    Email = "email@email.co.uk",
                    Password = "Password",
                    FirstName = "Joe",
                    LastName = "Blogs",
                    UserName = "JoeBlogs",
                    PhoneNumber = "1111111111",
                    Avatar = new byte[100],
                    Remember = false,
                    DateOfBirth = DateTime.Now.AddYears(-18)
                };

                var result = await service.Create(user);
                result.Should().NotBeNull();
            }
        }

        [Test]
        public async Task ShouldFailNonUniqueUser()
        {
            await using (var context = new DbContextFactory().CreateContext())
            {
                var service = new UserServiceFactory().Create(context);

                NewUserInput user = new NewUserInput()
                {
                    Email = "exists@email.co.uk",
                    Password = "Fail",
                    UserName = "ExistingUser"
                };

                FluentActions.Invoking(async () => await service.Create(user))
                    .Should().Throw<UserAlreadyExistsException>();
            }
        }

        // [Test]
        // [Ignore("Need to make separate mock")]
        // public async Task ShouldThrowDbUpdateException()
        // {
        //     MockContext.Setup(c => c.SaveChangesAsync(new CancellationToken()))
        //         .ReturnsAsync(() => throw new DbUpdateException());
        //
        //
        //     NewUserInput user = new NewUserInput()
        //     {
        //         Email = "email@email.co.uk",
        //         Password = "Password",
        //         FirstName = "Joe",
        //         LastName = "Blogs",
        //         UserName = "JoeBlogs",
        //         PhoneNumber = "1111111111",
        //         Avatar = new byte[100],
        //         Remember = false,
        //         DateOfBirth = DateTime.Now.AddYears(-18)
        //     };
        //
        //
        //     FluentActions.Invoking(async () => await UserService.Create(user)).Should().Throw<DbUpdateException>();
        // }
    }
}
using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace OpenEvent.Test.Services.UserService
{
    [TestFixture]
    public class Update
    {
        // [Test]
        // public async Task ShouldUpdateUser()
        // {
        //     var user = new User()
        //     {
        //         Id = new Guid("046E876E-D413-45AF-AC2A-552D7AA46C5C"),
        //         UserName = "updated"
        //     };
        //     
        //     var userService =
        //         new Web.Services.UserService(Context, new Logger<Web.Services.UserService>(new LoggerFactory()));
        //
        //     var updatedUser = await userService.Update(user);
        //     updatedUser.Should().Be(user);
        //
        //     // var check = await _context.Pages.FirstOrDefaultAsync(x => x.Id == page.Id);
        //     // check.Should().Be(page);
        // }
    }
}
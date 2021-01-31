using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace OpenEvent.Test.Services.UserService
{
    [TestFixture]
    public class EmailExists : UserTestFixture
    {
        [Test]
        public async Task EmailShouldExist()
        {
            var result = await UserService.EmailExists("exists@email.co.uk");
            result.Should().BeTrue();
        }
        
        [Test]
        public async Task EmailShouldNotExist()
        {
            var result = await UserService.EmailExists("doesntExist@email.co.uk");
            result.Should().BeFalse();
        }
    }
}
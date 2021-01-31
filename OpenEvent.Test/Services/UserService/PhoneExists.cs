using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace OpenEvent.Test.Services.UserService
{
    [TestFixture]
    public class PhoneExists : UserTestFixture
    {
        [Test]
        public async Task PhoneShouldExist()
        {
            var result = await UserService.PhoneExists("0000000000");
            result.Should().BeTrue();
        }
        
        [Test]
        public async Task PhoneShouldNotExist()
        {
            var result = await UserService.PhoneExists("1111111111");
            result.Should().BeFalse();
        }
    }
}
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using OpenEvent.Test.Factories;
using OpenEvent.Test.Setups;

namespace OpenEvent.Test.Services.UserService
{
    [TestFixture]
    public class PhoneExists
    {
        [Test]
        public async Task PhoneShouldExist()
        {
            await using (var context = new DbContextFactory().CreateContext())
            {
                var service = new UserServiceFactory().Create(context);

                var result = await service.PhoneExists("0000000000");
                result.Should().BeTrue();
            }
        }
        
        [Test]
        public async Task PhoneShouldNotExist()
        {
            await using (var context = new DbContextFactory().CreateContext())
            {
                var service = new UserServiceFactory().Create(context);

                var result = await service.PhoneExists("1111111111");
                result.Should().BeFalse();
            }
        }
    }
}
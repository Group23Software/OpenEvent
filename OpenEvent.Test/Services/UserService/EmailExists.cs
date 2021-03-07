using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using OpenEvent.Test.Factories;
using OpenEvent.Test.Setups;

namespace OpenEvent.Test.Services.UserService
{
    [TestFixture]
    public class EmailExists
    {
        [Test]
        public async Task EmailShouldExist()
        {
            await using (var context = new DbContextFactory().CreateContext())
            {
                var service = new UserServiceFactory().Create(context);

                var result = await service.EmailExists("exists@email.co.uk");
                result.Should().BeTrue();

            }
        }
        
        [Test]
        public async Task EmailShouldNotExist()
        {
            await using (var context = new DbContextFactory().CreateContext())
            {
                var service = new UserServiceFactory().Create(context);

                var result = await service.EmailExists("doesntExist@email.co.uk");
                result.Should().BeFalse();
            }
        }
    }
}
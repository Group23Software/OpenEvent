using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using OpenEvent.Data.Models.Category;
using OpenEvent.Test.Factories;
using OpenEvent.Test.Setups;

namespace OpenEvent.Test.Services.EventService
{
    [TestFixture]
    public class GetAllCategories
    {
        [Test]
        public async Task ShouldGetAllCategories()
        {
            await using (var context = new DbContextFactory().CreateContext())
            {
                var service = new EventServiceFactory().Create(context);
                
                var result = await service.GetAllCategories();
                result.Should().NotBeNull();
                result.Should().BeOfType<List<Category>>();
                result.Count.Should().BeGreaterThan(1);
            }
        }
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using OpenEvent.Web.Models.Category;

namespace OpenEvent.Test.Services.EventService
{
    [TestFixture]
    public class GetAllCategories : EventTestFixture
    {
        [Test]
        public async Task ShouldGetAllCategories()
        {
            var result = await EventService.GetAllCategories();
            result.Should().NotBeNull();
            result.Should().BeOfType<List<Category>>();
            result.Count.Should().BeGreaterThan(1);
        }
    }
}
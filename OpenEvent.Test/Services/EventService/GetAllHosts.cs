using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using OpenEvent.Data.Models.Event;
using OpenEvent.Test.Factories;
using OpenEvent.Test.Setups;

namespace OpenEvent.Test.Services.EventService
{
    [TestFixture]
    public class GetAllHosts
    {
        [Test]
        public async Task ShouldGetAllHostsEvents()
        {
            await using (var context = new DbContextFactory().CreateContext())
            {
                var service = new EventServiceFactory().Create(context);

                var result = await service.GetAllHosts(new Guid("046E876E-D413-45AF-AC2A-552D7AA46C5C"));
                result.Should().NotBeNull();
                result.Should().BeOfType<List<EventHostModel>>();
            }
        }
    }
}
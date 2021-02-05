using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using OpenEvent.Web.Models.Event;

namespace OpenEvent.Test.Services.EventService
{
    [TestFixture]
    public class GetAllHosts: EventTestFixture
    {
        [Test]
        public async Task ShouldGetAllHostsEvents()
        {
            var result = await EventService.GetAllHosts(new Guid("046E876E-D413-45AF-AC2A-552D7AA46C5C"));
            result.Should().NotBeNull();
            result.Should().BeOfType<List<EventHostModel>>();
        }
    }
}
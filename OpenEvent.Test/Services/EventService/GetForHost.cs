using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using OpenEvent.Web.Exceptions;
using OpenEvent.Web.Models.Event;

namespace OpenEvent.Test.Services.EventService
{
    [TestFixture]
    public class GetForHost : EventTestFixture
    {
        [Test]
        public async Task ShouldGetForHost()
        {
            var result = await EventService.GetForHost(new Guid("74831876-FC2E-4D03-99D8-B3872BDEFD5C"));
            result.Should().NotBeNull();
            result.Should().BeOfType<ActionResult<EventHostModel>>();
        }
        
        [Test]
        public async Task ShouldNotFindEvent()
        {
            FluentActions.Invoking(async () => await EventService.GetForHost(new Guid()))
                .Should().Throw<EventNotFoundException>();
        }
    }
}
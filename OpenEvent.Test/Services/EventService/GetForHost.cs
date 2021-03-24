using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using OpenEvent.Test.Factories;
using OpenEvent.Test.Setups;
using OpenEvent.Web.Exceptions;
using OpenEvent.Web.Models.Event;

namespace OpenEvent.Test.Services.EventService
{
    [TestFixture]
    public class GetForHost
    {
        [Test]
        public async Task ShouldGetForHost()
        {
            await using (var context = new DbContextFactory().CreateContext())
            {
                var service = new EventServiceFactory().Create(context);
                
                var result = await service.GetForHost(new Guid("74831876-FC2E-4D03-99D8-B3872BDEFD5C"));
                result.Should().NotBeNull();
                result.Should().BeOfType<EventHostModel>();
            }
        }
        
        [Test]
        public async Task ShouldNotFindEvent()
        {
            await using (var context = new DbContextFactory().CreateContext())
            {
                var service = new EventServiceFactory().Create(context);
                
                FluentActions.Invoking(async () => await service.GetForHost(new Guid()))
                    .Should().Throw<EventNotFoundException>();
            }
        }
    }
}
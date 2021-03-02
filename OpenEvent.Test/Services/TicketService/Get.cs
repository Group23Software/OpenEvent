using System;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using OpenEvent.Web.Exceptions;
using OpenEvent.Web.Models.Ticket;

namespace OpenEvent.Test.Services.TicketService
{
    [TestFixture]
    public class Get : TicketTestFixture
    {
        [Test]
        public async Task ShouldGetTicket()
        {
            var result = await TicketService.Get(new Guid("853F592D-D454-4FA1-BC9B-12991C13D835"));
            result.Should().NotBeNull();
            result.Should().BeOfType<TicketDetailModel>();
        }
        
        [Test]
        public async Task ShouldNotFindTicket()
        {
            FluentActions.Invoking(async () => await TicketService.Get(new Guid()))
                .Should().Throw<TicketNotFoundException>();
        }
    }
}
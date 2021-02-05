using System;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using OpenEvent.Web.Exceptions;
using OpenEvent.Web.Models.Ticket;

namespace OpenEvent.Test.Services.TestService
{
    [TestFixture]
    public class Get : TicketTestFixture
    {
        [Test]
        public async Task ShouldGetTicket()
        {
            var result = await TicketService.Get(new Guid("892C6AE2-0F9A-4125-9E95-FAC401A4EF60"));
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
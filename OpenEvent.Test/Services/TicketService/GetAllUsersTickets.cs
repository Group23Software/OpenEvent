using System;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace OpenEvent.Test.Services.TicketService
{
    [TestFixture]
    public class GetAllUsersTickets : TicketTestFixture
    {
        [Test]
        public async Task Should_Get_All_Users_Tickets()
        {
            var result = await TicketService.GetAllUsersTickets(new Guid("046E876E-D413-45AF-AC2A-552D7AA46C5C"));
            result.Count.Should().BeGreaterThan(0);
        }
    }
}
using System;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using OpenEvent.Test.Factories;
using OpenEvent.Test.Setups;

namespace OpenEvent.Test.Services.TicketService
{
    [TestFixture]
    public class GetAllUsersTickets
    {
        [Test]
        public async Task Should_Get_All_Users_Tickets()
        {
            await using (var context = new DbContextFactory().CreateContext())
            {
                var service = new TicketServiceFactory().Create(context);

                var result = await service.GetAllUsersTickets(new Guid("046E876E-D413-45AF-AC2A-552D7AA46C5C"));
                result.Count.Should().BeGreaterThan(0);
            }
        }
    }
}
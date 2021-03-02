using System;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using OpenEvent.Web.Models.Ticket;

namespace OpenEvent.Test.Services.TicketService
{
    [TestFixture]
    public class VerifyTicket : TicketTestFixture
    {
        private readonly Guid EventId = new Guid("74831876-FC2E-4D03-99D8-B3872BDEFD5C");

        [Test]
        public async Task Should_Verify_Ticket()
        {
            FluentActions.Invoking(async () => await TicketService.VerifyTicket(new TicketVerifyBody()
                {Id = new Guid("853F592D-D454-4FA1-BC9B-12991C13D835"), EventId = EventId})).Should().NotThrow();
        }

        [Test]
        public async Task Should_Be_Unauthorised_Ticket()
        {
            FluentActions.Invoking(async () =>
                    await TicketService.VerifyTicket(new TicketVerifyBody() {Id = new Guid(), EventId = EventId}))
                .Should()
                .Throw<UnauthorizedAccessException>();
        }
    }
}
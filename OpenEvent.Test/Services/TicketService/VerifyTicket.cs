using System;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using OpenEvent.Data.Models.Ticket;
using OpenEvent.Test.Factories;
using OpenEvent.Test.Setups;

namespace OpenEvent.Test.Services.TicketService
{
    [TestFixture]
    public class VerifyTicket
    {
        private readonly Guid EventId = new Guid("74831876-FC2E-4D03-99D8-B3872BDEFD5C");

        [Test]
        public async Task Should_Verify_Ticket()
        {
            await using (var context = new DbContextFactory().CreateContext())
            {
                var service = new TicketServiceFactory().Create(context);
                
                FluentActions.Invoking(async () => await service.VerifyTicket(new TicketVerifyBody()
                    {Id = new Guid("A85DDDF9-C5ED-469C-914F-75097B950024"), EventId = EventId})).Should().NotThrow();
            }
        }

        [Test]
        public async Task Should_Be_Unauthorised_Ticket()
        {
            await using (var context = new DbContextFactory().CreateContext())
            {
                var service = new TicketServiceFactory().Create(context);

                FluentActions.Invoking(async () =>
                        await service.VerifyTicket(new TicketVerifyBody() {Id = new Guid(), EventId = EventId}))
                    .Should()
                    .Throw<UnauthorizedAccessException>();
            }
        }
    }
}
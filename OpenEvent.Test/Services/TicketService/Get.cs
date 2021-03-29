using System;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using OpenEvent.Data.Models.Ticket;
using OpenEvent.Test.Factories;
using OpenEvent.Test.Setups;
using OpenEvent.Web.Exceptions;

namespace OpenEvent.Test.Services.TicketService
{
    [TestFixture]
    public class Get
    {
        [Test]
        public async Task Should_Get_Ticket()
        {
            await using (var context = new DbContextFactory().CreateContext())
            {
                var service = new TicketServiceFactory().Create(context);
                
                var result = await service.Get(new Guid("A85DDDF9-C5ED-469C-914F-75097B950024"));
                result.Should().NotBeNull();
                result.QRCode.Should().NotBeNull();
                result.Should().BeOfType<TicketDetailModel>();
            }
        }
        
        [Test]
        public async Task Should_Not_Find_Ticket()
        {
            await using (var context = new DbContextFactory().CreateContext())
            {
                var service = new TicketServiceFactory().Create(context);

                FluentActions.Invoking(async () => await service.Get(new Guid()))
                    .Should().Throw<TicketNotFoundException>();
            }
        }
    }
}
using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using OpenEvent.Web.Exceptions;
using OpenEvent.Web.Models.Address;
using OpenEvent.Web.Models.Event;

namespace OpenEvent.Test.Services.EventService
{
    [TestFixture]
    public class Update : EventTestFixture
    {
        private readonly UpdateEventBody UpdateEventBody = new UpdateEventBody()
        {
            Id = new Guid("74831876-FC2E-4D03-99D8-B3872BDEFD5C"),
            Name = "Updated name",
            Address = new Address()
            {
                AddressLine1 = "Updated Main Street",
                AddressLine2 = "",
                City = "City of update",
                CountryCode = "GB",
                CountryName = "United Kingdom",
                PostalCode = "BB2 2BB"
            },
            Description = "This is an updated test event"
        };

        [Test]
        public async Task ShouldUpdate()
        {
            await EventService.Update(UpdateEventBody);

            var check = await MockContext.Object.Events.FirstOrDefaultAsync(x => x.Id == UpdateEventBody.Id);
            check.Name.Should().Be(UpdateEventBody.Name);
            check.Description.Should().Be(UpdateEventBody.Description);
            check.Address.Should().Be(UpdateEventBody.Address);
        }

        [Test]
        public async Task ShouldNotFindEvent()
        {
            FluentActions.Invoking(async () => await EventService.Update(new UpdateEventBody {Id = new Guid()}))
                .Should().Throw<EventNotFoundException>();
        }

        [Test]
        public async Task ShouldThrowDbUpdateException()
        {
            MockContext.Setup(c => c.SaveChangesAsync(new CancellationToken()))
                .ReturnsAsync(() => throw new DbUpdateException());

            FluentActions.Invoking(async () => await EventService.Update(UpdateEventBody)).Should()
                .Throw<DbUpdateException>();
        }
    }
}
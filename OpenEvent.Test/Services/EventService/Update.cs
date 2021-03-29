using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using OpenEvent.Data.Models.Address;
using OpenEvent.Data.Models.Event;
using OpenEvent.Test.Factories;
using OpenEvent.Test.Setups;
using OpenEvent.Web.Exceptions;

namespace OpenEvent.Test.Services.EventService
{
    [TestFixture]
    public class Update
    {
        private readonly UpdateEventBody UpdateEventBody = new()
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
            await using (var context = new DbContextFactory().CreateContext())
            {
                var service = new EventServiceFactory().Create(context);

                await service.Update(UpdateEventBody);

                var check = await context.Events.FirstOrDefaultAsync(x => x.Id == UpdateEventBody.Id);
                check.Name.Should().Be(UpdateEventBody.Name);
                check.Description.Should().Be(UpdateEventBody.Description);
                check.Address.Should().Be(UpdateEventBody.Address);
            }
        }

        [Test]
        public async Task ShouldNotFindEvent()
        {
            await using (var context = new DbContextFactory().CreateContext())
            {
                var service = new EventServiceFactory().Create(context);

                FluentActions.Invoking(async () => await service.Update(new UpdateEventBody {Id = new Guid()}))
                    .Should().Throw<EventNotFoundException>();
            }
        }

        // [Test]
        // [Ignore("Need to make separate mock")]
        // public async Task ShouldThrowDbUpdateException()
        // {
        //     MockContext.Setup(c => c.SaveChangesAsync(new CancellationToken()))
        //         .ReturnsAsync(() => throw new DbUpdateException());
        //
        //     FluentActions.Invoking(async () => await service.Update(UpdateEventBody)).Should()
        //         .Throw<DbUpdateException>();
        // }
    }
}
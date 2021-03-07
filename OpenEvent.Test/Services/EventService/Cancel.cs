using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using OpenEvent.Test.Factories;
using OpenEvent.Test.Setups;
using OpenEvent.Web.Exceptions;

namespace OpenEvent.Test.Services.EventService
{
    [TestFixture]
    public class Cancel
    {
        private readonly Guid RealEventId = new Guid("74831876-FC2E-4D03-99D8-B3872BDEFD5C");

        [Test]
        public async Task ShouldCancelEvent()
        {
            await using (var context = new DbContextFactory().CreateContext())
            {
                var service = new EventServiceFactory().Create(context);
                
                await service.Cancel(RealEventId);
                var result = await context.Events.FirstOrDefaultAsync(x => x.Id == RealEventId);
                result.isCanceled.Should().BeTrue();
            }
        }

        [Test]
        public async Task ShouldNotFindUser()
        {
            await using (var context = new DbContextFactory().CreateContext())
            {
                var service = new EventServiceFactory().Create(context);
                
                FluentActions.Invoking(async () => await service.Cancel(Guid.NewGuid()))
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
        //     FluentActions.Invoking(async () => await EventService.Cancel(RealEventId)).Should().Throw<DbUpdateException>();
        // }
    }
}
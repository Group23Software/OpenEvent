using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using OpenEvent.Web.Exceptions;

namespace OpenEvent.Test.Services.EventService
{
    [TestFixture]
    public class Cancel: EventTestFixture
    {

        private readonly Guid RealEventId = new Guid("74831876-FC2E-4D03-99D8-B3872BDEFD5C");
        
        [Test]
        public async Task ShouldCancelEvent()
        {
            await EventService.Cancel(RealEventId);
            var result = await MockContext.Object.Events.FirstOrDefaultAsync(x => x.Id == RealEventId);
            result.isCanceled.Should().BeTrue();
        }
        
        [Test]
        public async Task ShouldNotFindUser()
        {
            FluentActions.Invoking(async () => await EventService.Cancel(Guid.NewGuid()))
                .Should().Throw<EventNotFoundException>();
        }
        
        [Test]
        public async Task ShouldThrowDbUpdateException()
        {
            MockContext.Setup(c => c.SaveChangesAsync(new CancellationToken()))
                .ReturnsAsync(() => throw new DbUpdateException());

            FluentActions.Invoking(async () => await EventService.Cancel(RealEventId)).Should().Throw<DbUpdateException>();
        }
    }
}
using System;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenEvent.Data.Models.Intent;
using OpenEvent.Test.Factories;
using OpenEvent.Test.Setups;

namespace OpenEvent.Test.Services.TransactionService
{
    public class CreateIntent
    {
        [Test]
        public async Task Should_Create()
        {
            await using (var context = new DbContextFactory().CreateContext())
            {
                var service = new TransactionServiceFactory().Create(context);

                var result = service.CreateIntent(new CreateIntentBody()
                {
                    Amount = 100,
                    UserId = new Guid("046E876E-D413-45AF-AC2A-552D7AA46C5C"),
                    EventId = new Guid("74831876-FC2E-4D03-99D8-B3872BDEFD5C"),
                });
            }
        }
    }
}
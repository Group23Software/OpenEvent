using System;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenEvent.Data.Models.Intent;
using OpenEvent.Test.Factories;
using OpenEvent.Test.Setups;

namespace OpenEvent.Test.Services.TransactionService
{
    public class InjectPaymentMethod
    {
        [Test]
        public async Task Should_Inject()
        {
            await using (var context = new DbContextFactory().CreateContext())
            {
                var service = new TransactionServiceFactory().Create(context);

                var result = service.InjectPaymentMethod(new InjectPaymentMethodBody()
                {
                    UserId = new Guid("046E876E-D413-45AF-AC2A-552D7AA46C5C"),
                    IntentId = "pi_1IQ7gqK2ugLXrgQXpxZscFpi",
                    CardId = "card_1IPw30K2ugLXrgQX4NDVSrfJ"
                });
            }
        }
    }
}
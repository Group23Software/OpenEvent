using System;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenEvent.Web.Models.Intent;

namespace OpenEvent.Test.Services.TransactionService
{
    public class InjectPaymentMethod : TransactionTestFixture
    {
        [Test]
        public async Task Should_Inject()
        {
            var result = TransactionService.InjectPaymentMethod(new InjectPaymentMethodBody()
            {
                UserId = new Guid("046E876E-D413-45AF-AC2A-552D7AA46C5C"),
                IntentId = "pi_1IQ7gqK2ugLXrgQXpxZscFpi",
                CardId = "card_1IPw30K2ugLXrgQX4NDVSrfJ"
            });
        }
    }
}
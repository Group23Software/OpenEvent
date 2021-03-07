using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using OpenEvent.Test.Factories;
using OpenEvent.Test.Setups;
using OpenEvent.Web.Models.Address;
using OpenEvent.Web.Models.PaymentMethod;
using OpenEvent.Web.Models.User;

namespace OpenEvent.Test.Services.PaymentService
{
    [TestFixture]
    public class AddPaymentMethod
    {
        [Test]
        public async Task Should_Create_Customer_If_Null()
        {
            await using (var context = new DbContextFactory().CreateContext())
            {
                var service = new PaymentServiceFactory().Create(context);

                Guid userId = new Guid("046E876E-D413-45AF-AC2A-552D7AA46C5C");
                var result = service.AddPaymentMethod(new AddPaymentMethodBody()
                {
                    UserId = userId,
                    CardToken = "tok_visa_debit"
                });
                result.Should().NotBeNull();
                var user = context.Users.First(x => x.Id == userId);
                user.StripeCustomerId.Should().NotBeNull();
            }
        }
    }
}
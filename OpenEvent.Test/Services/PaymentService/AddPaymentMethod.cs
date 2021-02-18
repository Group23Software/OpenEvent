using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using OpenEvent.Web.Models.Address;
using OpenEvent.Web.Models.PaymentMethod;
using OpenEvent.Web.Models.User;

namespace OpenEvent.Test.Services.PaymentService
{
    [TestFixture]
    public class AddPaymentMethod : PaymentTestFixture
    {
        [Test]
        public async Task Should_Create_Customer_If_Null()
        {
            Guid userId = new Guid("046E876E-D413-45AF-AC2A-552D7AA46C5C");
            var result = PaymentService.AddPaymentMethod(new AddPaymentMethodModel()
            {
                UserId = userId,
                CardToken = "tok_visa_debit"
            });
            result.Should().NotBeNull();
            var user = MockContext.Object.Users.First(x => x.Id == userId);
        }
    }
}
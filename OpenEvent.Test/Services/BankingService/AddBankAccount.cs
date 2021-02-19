using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using OpenEvent.Web.Models.BankAccount;

namespace OpenEvent.Test.Services.BankingService
{
    [TestFixture]
    public class AddBankAccount : BankingTestFixture
    {
        [Test]
        public async Task Should_Create_Account_If_Null()
        {
            Guid userId = new Guid("046E876E-D413-45AF-AC2A-552D7AA46C5C");
            var result = BankingService.AddBankAccount(new AddBankAccountBody()
            {
                UserId = userId,
                BankToken = "btok_gb"
            });
            result.Should().NotBeNull();
            var user = MockContext.Object.Users.First(x => x.Id == userId);
            user.StripeAccountId.Should().NotBeNull();
        }
    }
}
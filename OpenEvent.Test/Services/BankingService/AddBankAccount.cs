using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using OpenEvent.Test.Factories;
using OpenEvent.Test.Setups;
using OpenEvent.Web.Exceptions;
using OpenEvent.Web.Models.BankAccount;

namespace OpenEvent.Test.Services.BankingService
{
    [TestFixture]
    public class AddBankAccount
    {
        private readonly Guid userId = new Guid("046E876E-D413-45AF-AC2A-552D7AA46C5C");

        [Test]
        public async Task Should_Create_Account_If_Null()
        {
            await using (var context = new DbContextFactory().CreateContext())
            {
                var service = new BankingServiceFactory().Create(context);

                var result = service.AddBankAccount(new AddBankAccountBody()
                {
                    UserId = userId,
                    BankToken = "btok_gb"
                });
                result.Should().NotBeNull();
                var user = context.Users.First(x => x.Id == userId);
                user.StripeAccountId.Should().NotBeNull();
            }
        }

        [Test]
        public async Task Should_Not_Find_User()
        {
            await using (var context = new DbContextFactory().CreateContext())
            {
                var service = new BankingServiceFactory().Create(context);

                FluentActions.Invoking(async () =>
                        await service.AddBankAccount(new AddBankAccountBody() {UserId = new Guid()}))
                    .Should().Throw<UserNotFoundException>();
            }
        }

        // [Test]
        // [Ignore("Need to make separate mock")]
        // public async Task Should_Throw_Db_Update_Exception()
        // {
        //     MockContext.Setup(c => c.SaveChangesAsync(new CancellationToken()))
        //         .ReturnsAsync(() => throw new DbUpdateException());
        //
        //     FluentActions.Invoking(async () =>
        //             await BankingService.AddBankAccount(new AddBankAccountBody()
        //             {
        //                 UserId = userId,
        //                 BankToken = "btok_gb"
        //             }))
        //         .Should().Throw<DbUpdateException>();
        // }
    }
}
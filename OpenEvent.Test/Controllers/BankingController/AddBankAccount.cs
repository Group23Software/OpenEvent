using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using OpenEvent.Data.Models.BankAccount;
using OpenEvent.Web.Exceptions;

namespace OpenEvent.Test.Controllers.BankingController
{
    [TestFixture]
    public class AddBankAccount
    {
        private readonly Mock<Web.Services.IBankingService> BankingService = new();
        private Web.Controllers.BankingController BankingController;

        private readonly AddBankAccountBody AddBankAccountBody = new()
        {
            BankToken = "Test bank token"
        };
        
        private readonly AddBankAccountBody UserNotFoundBody = new()
        {
            BankToken = "Not there"
        };

        private readonly AddBankAccountBody SaveErrorBody = new()
        {
            BankToken = "Save error token"
        };
        
        [SetUp]
        public async Task Setup()
        {
            BankingService.Setup(x => x.AddBankAccount(AddBankAccountBody));
            BankingService.Setup(x => x.AddBankAccount(UserNotFoundBody)).ThrowsAsync(new UserNotFoundException());
            BankingService.Setup(x => x.AddBankAccount(SaveErrorBody)).ThrowsAsync(new DbUpdateException());
            BankingController = new Web.Controllers.BankingController(BankingService.Object,
                new Mock<ILogger<Web.Controllers.BankingController>>().Object);
        }

        [Test]
        public async Task Should_Add_Payment_Method()
        {
            var result = await BankingController.AddBankAccount(AddBankAccountBody);
            result.Should().BeOfType<ActionResult<BankAccountViewModel>>();
        }
        
        [Test]
        public async Task Should_Not_Find_User()
        {
            var result = await BankingController.AddBankAccount(UserNotFoundBody);
            result.Result.Should().BeOfType<BadRequestObjectResult>().Subject.Value.Should().BeOfType<UserNotFoundException>();
        }

        [Test]
        public async Task Should_Throw_Db_Update_Exception()
        {
            var result = await BankingController.AddBankAccount(SaveErrorBody);
            result.Result.Should()
                .BeOfType<BadRequestObjectResult>()
                .Subject.Value.Should().BeOfType<DbUpdateException>();
        }
    }
}
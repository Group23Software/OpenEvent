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
    public class RemoveBankAccount
    {
        private readonly Mock<Web.Services.IBankingService> BankingService = new();
        private Web.Controllers.BankingController BankingController;

        private readonly RemoveBankAccountBody RemoveBankAccountBody = new()
        {
            BankId = "Test bank id"
        };

        private readonly RemoveBankAccountBody SaveErrorBody = new()
        {
            BankId = "Save error"
        };
        
        private readonly RemoveBankAccountBody UserNotFoundBody = new()
        {
            BankId = "Not there"
        };


        [SetUp]
        public async Task Setup()
        {
            BankingService.Setup(x => x.RemoveBankAccount(RemoveBankAccountBody));
            BankingService.Setup(x => x.RemoveBankAccount(UserNotFoundBody)).ThrowsAsync(new UserNotFoundException());
            BankingService.Setup(x => x.RemoveBankAccount(SaveErrorBody)).ThrowsAsync(new DbUpdateException());
            BankingController = new Web.Controllers.BankingController(BankingService.Object,
                new Mock<ILogger<Web.Controllers.BankingController>>().Object);
        }

        [Test]
        public async Task Should_Remove_Payment_Method()
        {
            var result = await BankingController.RemoveBankAccount(RemoveBankAccountBody);
            result.Should().BeOfType<OkResult>();
        }

        [Test]
        public async Task Should_Not_Find_User()
        {
            var result = await BankingController.RemoveBankAccount(UserNotFoundBody);
            result.Should().BeOfType<BadRequestObjectResult>().Subject.Value.Should()
                .BeOfType<UserNotFoundException>();
        }

        [Test]
        public async Task Should_Throw_Db_Update_Exception()
        {
            var result = await BankingController.RemoveBankAccount(SaveErrorBody);
            result.Should()
                .BeOfType<BadRequestObjectResult>()
                .Subject.Value.Should().BeOfType<DbUpdateException>();
        }
    }
}
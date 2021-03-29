using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using OpenEvent.Data.Models.PaymentMethod;
using OpenEvent.Web.Exceptions;

namespace OpenEvent.Test.Controllers.PaymentController
{
    [TestFixture]
    public class MakePaymentDefault
    {
        private readonly Mock<Web.Services.IPaymentService> PaymentService = new();
        private Web.Controllers.PaymentController PaymentController;

        private readonly MakeDefaultBody MakeDefaultBody = new()
        {
            PaymentId = "test payment id"
        };

        private readonly MakeDefaultBody SaveErrorBody = new()
        {
            PaymentId = "Should throw error"
        };
        
        private readonly MakeDefaultBody ErrorBody = new()
        {
            PaymentId = "Error"
        };


        [SetUp]
        public async Task Setup()
        {
            PaymentService.Setup(x => x.MakeDefault(MakeDefaultBody));
            PaymentService.Setup(x => x.MakeDefault(ErrorBody)).ThrowsAsync(new UserNotFoundException());
            PaymentService.Setup(x => x.MakeDefault(SaveErrorBody)).ThrowsAsync(new DbUpdateException());
            
            PaymentController = new Web.Controllers.PaymentController(PaymentService.Object,
                new Mock<ILogger<Web.Controllers.PaymentController>>().Object);
        }

        [Test]
        public async Task Should_Add_Payment_Method()
        {
            var result = await PaymentController.MakePaymentDefault(MakeDefaultBody);
            result.Should().BeOfType<OkResult>();
        }

        [Test]
        public async Task Should_Not_Find_User()
        {
            var result = await PaymentController.MakePaymentDefault(ErrorBody);
            result.Should().BeOfType<BadRequestObjectResult>().Subject.Value.Should()
                .BeOfType<UserNotFoundException>();
        }

        [Test]
        public async Task Should_Throw_Db_Update_Exception()
        {
            var result = await PaymentController.MakePaymentDefault(SaveErrorBody);
            result.Should()
                .BeOfType<BadRequestObjectResult>()
                .Subject.Value.Should().BeOfType<DbUpdateException>();
        }
    }
}
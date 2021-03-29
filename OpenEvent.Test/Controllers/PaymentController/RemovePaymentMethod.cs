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
    public class RemovePaymentMethod
    {
        private readonly Mock<Web.Services.IPaymentService> PaymentService = new();
        private Web.Controllers.PaymentController PaymentController;

        private readonly RemovePaymentMethodBody RemovePaymentMethodBody = new()
        {
            PaymentId = "Test payment id"
        };

        private readonly RemovePaymentMethodBody SaveErrorBody = new()
        {
            PaymentId = "Save error"
        };
        
        private readonly RemovePaymentMethodBody ErrorBody = new()
        {
            PaymentId = "Error"
        };

        [SetUp]
        public async Task Setup()
        {
            PaymentService.Setup(x => x.RemovePaymentMethod(RemovePaymentMethodBody));
            PaymentService.Setup(x => x.RemovePaymentMethod(ErrorBody)).ThrowsAsync(new UserNotFoundException());
            PaymentService.Setup(x => x.RemovePaymentMethod(SaveErrorBody)).ThrowsAsync(new DbUpdateException());
            
            PaymentController = new Web.Controllers.PaymentController(PaymentService.Object,
                new Mock<ILogger<Web.Controllers.PaymentController>>().Object);
        }

        [Test]
        public async Task Should_Remove_Payment_Method()
        {
            var result = await PaymentController.RemovePaymentMethod(RemovePaymentMethodBody);
            result.Should().BeOfType<OkResult>();
        }

        [Test]
        public async Task Should_Not_Find_User()
        {
            var result = await PaymentController.RemovePaymentMethod(ErrorBody);
            result.Should().BeOfType<BadRequestObjectResult>().Subject.Value.Should()
                .BeOfType<UserNotFoundException>();
        }

        [Test]
        public async Task Should_Throw_Db_Update_Exception()
        {
            var result = await PaymentController.RemovePaymentMethod(SaveErrorBody);
            result.Should()
                .BeOfType<BadRequestObjectResult>()
                .Subject.Value.Should().BeOfType<DbUpdateException>();
        }
    }
}
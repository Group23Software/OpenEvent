using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using OpenEvent.Web.Exceptions;
using OpenEvent.Web.Models.PaymentMethod;

namespace OpenEvent.Test.Controllers.PaymentController
{
    [TestFixture]
    public class AddPaymentMethod
    {
        private readonly Mock<Web.Services.IPaymentService> PaymentService = new();
        private Web.Controllers.PaymentController PaymentController;

        private readonly AddPaymentMethodBody AddPaymentMethodBody = new()
        {
            NickName = "Add payment method"
        };
        
        private readonly AddPaymentMethodBody SaveErrorBody = new()
        {
            NickName = "Should throw error"
        };


        [SetUp]
        public async Task Setup()
        {
            PaymentService.Setup(x => x.AddPaymentMethod(AddPaymentMethodBody));
            PaymentService.Setup(x => x.AddPaymentMethod(null)).ThrowsAsync(new UserNotFoundException());
            PaymentService.Setup(x => x.AddPaymentMethod(SaveErrorBody)).ThrowsAsync(new DbUpdateException());
            PaymentController = new Web.Controllers.PaymentController(PaymentService.Object,
                new Mock<ILogger<Web.Controllers.PaymentController>>().Object);
        }

        [Test]
        public async Task Should_Add_Payment_Method()
        {
            var result = await PaymentController.AddPaymentMethod(AddPaymentMethodBody);
            result.Should().BeOfType<ActionResult<PaymentMethodViewModel>>();
        }
        
        [Test]
        public async Task Should_Not_Find_User()
        {
            var result = await PaymentController.AddPaymentMethod(null);
            result.Result.Should().BeOfType<BadRequestObjectResult>().Subject.Value.Should().BeOfType<UserNotFoundException>();
        }

        [Test]
        public async Task Should_Throw_Db_Update_Exception()
        {
            var result = await PaymentController.AddPaymentMethod(SaveErrorBody);
            result.Result.Should()
                .BeOfType<BadRequestObjectResult>()
                .Subject.Value.Should().BeOfType<DbUpdateException>();
        }
    }
}
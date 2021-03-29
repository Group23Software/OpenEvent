using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using OpenEvent.Data.Models.Ticket;
using OpenEvent.Web.Exceptions;

namespace OpenEvent.Test.Controllers.TicketController
{
    [TestFixture]
    public class Get
    {
        private readonly Mock<Web.Services.ITicketService> TicketServiceMock = new();

        private Web.Controllers.TicketController TicketController;

        private readonly TicketDetailModel TicketDetailModel = new TicketDetailModel()
        {
            Id = new Guid("3BDF91F8-1A80-48E8-B656-F6D4799A0CB9"),
            QRCode = "Code",
        };
        [SetUp]
        public async Task Setup()
        {
            TicketServiceMock.Setup(x => x.Get(TicketDetailModel.Id)).ReturnsAsync(TicketDetailModel);
            TicketServiceMock.Setup(x => x.Get(new Guid())).ThrowsAsync(new TicketNotFoundException());
            TicketController = new Web.Controllers.TicketController(TicketServiceMock.Object,
                new Mock<ILogger<Web.Controllers.TicketController>>().Object);
        }

        [Test]
        public async Task ShouldGetTicket()
        {
            var result = await TicketController.Get(TicketDetailModel.Id);
            result.Should().BeOfType<ActionResult<TicketDetailModel>>().Subject.Value.Should().Be(TicketDetailModel);
        }

        [Test]
        public async Task ShouldNotGetTicket()
        {
            var result = await TicketController.Get(new Guid());
            result.Result.Should().BeOfType<BadRequestObjectResult>().Subject.Value.Should().BeOfType<TicketNotFoundException>();
        }
    }
}
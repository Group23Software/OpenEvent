using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using OpenEvent.Web;
using OpenEvent.Web.Services;

namespace OpenEvent.Test.Services.EmailService
{
    public class EmailTestFixture
    {
        private IOptions<AppSettings> AppSettings;
        private Mock<IWebHostEnvironment> WebHostEnvironmentMock;
        protected IEmailService EmailService;
        
        [SetUp]
        public async Task Setup()
        {
            AppSettings = Options.Create(new AppSettings()
            {
                MailSettings = new MailSettings()
                {
                    Server = "smtp.gmail.com",
                    Port = 465,
                    SenderName = "OpenEvent",
                    SenderEmail = "openeventmail@gmail.com",
                    Username = "openeventmail@gmail.com",
                    Password = "81wlkmfBBZoI"
                }
            });

            WebHostEnvironmentMock = new Mock<IWebHostEnvironment>();
            WebHostEnvironmentMock.Setup(x => x.EnvironmentName).Returns("Development");

            EmailService = new Web.Services.EmailService(AppSettings, WebHostEnvironmentMock.Object);
        }
    }
}
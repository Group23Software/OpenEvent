using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Moq;
using OpenEvent.Web;
using OpenEvent.Web.Contexts;
using OpenEvent.Web.Services;

namespace OpenEvent.Test.Factories
{
    public class EmailServiceFactory
    {
        public EmailService Create()
        {
            var appSettings = Options.Create(new AppSettings()
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

            var webHostEnvironmentMock = new Mock<IWebHostEnvironment>();
            webHostEnvironmentMock.Setup(x => x.EnvironmentName).Returns("Development");

            return new EmailService(appSettings, webHostEnvironmentMock.Object);
        }
    }
}
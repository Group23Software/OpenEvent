using System;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MimeKit;

namespace OpenEvent.Web.Services
{
    public interface IEmailService
    {
        Task SendAsync(string email, string name, string body, string subject);
    }

    public class EmailService : IEmailService
    {
        private readonly AppSettings AppSettings;
        private readonly IWebHostEnvironment Environment;

        public EmailService(IOptions<AppSettings> appSettings, IWebHostEnvironment env)
        {
            AppSettings = appSettings.Value;
            Environment = env;
        }

        public async Task SendAsync(string email, string name, string body, string subject)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(AppSettings.MailSettings.SenderName,
                    AppSettings.MailSettings.SenderEmail));
                message.To.Add(new MailboxAddress(name, email));

                message.Subject = subject;
                
                var builder = new BodyBuilder {HtmlBody = body};
                message.Body = builder.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    client.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;

                    if (Environment.IsDevelopment())
                    {
                        await client.ConnectAsync(AppSettings.MailSettings.Server, AppSettings.MailSettings.Port, true);
                    }
                    else
                    {
                        await client.ConnectAsync(AppSettings.MailSettings.Server);
                    }

                    await client.AuthenticateAsync(AppSettings.MailSettings.Username,
                        AppSettings.MailSettings.Password);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message);
            }
        }
    }
}
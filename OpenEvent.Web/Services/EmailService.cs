using System;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MimeKit;

namespace OpenEvent.Web.Services
{
    /// <summary>
    /// Service for sending emails
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Method that sends an email asynchronously
        /// </summary>
        /// <param name="email">recipients email</param>
        /// <param name="body">email body</param>
        /// <param name="subject">subject of the email</param>
        /// <returns>Completed task one the email has sent</returns>
        Task SendAsync(string email, string body, string subject);
    }

    /// <inheritdoc />
    public class EmailService : IEmailService
    {
        private readonly AppSettings AppSettings;
        private readonly IWebHostEnvironment Environment;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="appSettings"></param>
        /// <param name="env"></param>
        public EmailService(IOptions<AppSettings> appSettings, IWebHostEnvironment env)
        {
            AppSettings = appSettings.Value;
            Environment = env;
        }
        
        /// <inheritdoc />
        /// <exception cref="InvalidOperationException">Thrown when the email fails to send</exception>
        public async Task SendAsync(string email, string body, string subject)
        {
            try
            {
                var message = new MimeMessage();

                message.From.Add(new MailboxAddress(AppSettings.MailSettings.SenderName,
                    AppSettings.MailSettings.SenderEmail));
                message.To.Add(new MailboxAddress("OpenEvent", email));

                message.Subject = subject;

                var builder = new BodyBuilder {HtmlBody = body};
                message.Body = builder.ToMessageBody();

                using var client = new SmtpClient
                {
                    ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true
                };

                // if in development set port
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
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Backend.Infrastructure.Email.Abstraction;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Backend.Infrastructure.Email
{
    public class SendGridEmailService : IEmailService
    {
        private readonly SendGridOptions _config;

        public SendGridEmailService(IOptions<SendGridOptions> configuration) => _config = configuration.Value;

        public async Task Send(string subject, string text, string receiver)
        {
            var client = new SendGridClient(_config.ApiKey);
            var from = new EmailAddress(_config.SenderEmail, _config.SenderDisplayName);
            var receivers = new List<EmailAddress> { new EmailAddress(receiver) };
            SendGridMessage mail = MailHelper.CreateSingleEmailToMultipleRecipients(from, receivers, subject, text, text, false);

            // TODO: This shall be saved to the database and processed async.
            Response response = await client.SendEmailAsync(mail);
            if (response.StatusCode != HttpStatusCode.Accepted)
            {
                string result = await response.Body.ReadAsStringAsync();
                throw new InvalidOperationException("Email sending was not successful. Body: " + result);
            }
        }
    }
}
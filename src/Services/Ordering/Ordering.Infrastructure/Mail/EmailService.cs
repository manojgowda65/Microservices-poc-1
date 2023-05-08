using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Models;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Mail
{
    public class EmailService : IEmailService
    {
        public EmailSettings _emailSettings { get; }
        public ILogger<EmailService> _logger { get;  }
        public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
        {
            _emailSettings = emailSettings.Value;
            _logger=logger; 
        }

        public async Task<bool> SendMail(Email mail)
        {
            var client = new SendGridClient(_emailSettings.ApiKey);
            var from_email = new EmailAddress(_emailSettings.FromAddress, _emailSettings.FromName);
            var subject = mail.Subject;
            var to_email = new EmailAddress(mail.To, mail.To);
            var body = mail.Body;
            var sendGridMessage = MailHelper.CreateSingleEmail(from_email,to_email,subject,body,body);
            var response = await client.SendEmailAsync(sendGridMessage);


            if (response.StatusCode == System.Net.HttpStatusCode.OK || response.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
                _logger.LogInformation("Email sent");
                return true;
            }

            _logger.LogError("Email sending failed");
            return false;
        }
    }
}

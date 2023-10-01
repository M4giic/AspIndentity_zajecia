using Mailjet.Client;
using Mailjet.Client.Resources;
using Mailjet.Client.TransactionalEmails;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace IdentityNetCore.Service
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly IMailjetClient _mailjetClient;
        private readonly ILogger<IEmailSender> _logger;

        public SmtpEmailSender(IMailjetClient mailjetClient, ILogger<IEmailSender> logger)
        {
            _mailjetClient = mailjetClient;
            _logger = logger;
        }

        public async Task SendEmailAsync(string fromAddress, string toAddress, string subject, string message)
        {
            _logger.LogInformation($"Sending email to {toAddress} with message: {message}");
            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource
            };

            var email = new TransactionalEmailBuilder()
                   .WithFrom(new SendContact(fromAddress))
                   .WithSubject(subject)
                   .WithHtmlPart(message)
                   .WithTo(new SendContact(toAddress))
                   .Build();

            var response = await _mailjetClient.SendTransactionalEmailAsync(email);

            foreach (var responseMessage in response.Messages)
            {
                _logger.LogInformation($"Sending email response status: {responseMessage.Status}");
            }
        }
    }
}

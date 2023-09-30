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

        public SmtpEmailSender(IMailjetClient mailjetClient)
        {
            _mailjetClient = mailjetClient;
        }

        public async Task SendEmailAsync(string fromAddress, string toAddress, string subject, string message)
        {
            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource
            };

            var email = new TransactionalEmailBuilder()
                   .WithFrom(new SendContact("from@test.com"))
                   .WithSubject("Test subject")
                   .WithHtmlPart("<h1>Header</h1>")
                   .WithTo(new SendContact("to@test.com"))
                   .Build();

            var response = await _mailjetClient.SendTransactionalEmailAsync(email);

        }
    }
}

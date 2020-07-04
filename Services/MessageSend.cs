using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Urban.ng.Services
{
    public class MessageSend : IEmailSend
    {
        public MessageSend(IOptions<MessageSenderOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public MessageSenderOptions Options { get; set; }
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var myMessage = new SendGridMessage();
            myMessage.AddTo(email);
            myMessage.From = new EmailAddress("info@technicians.ng", "Technicians.ng");
            myMessage.Subject = subject;
            myMessage.PlainTextContent = message;
            myMessage.HtmlContent = message;

            var apiKey = Options.SendGridApiKey;

            var transportWeb = new SendGridClient(apiKey);

            return transportWeb.SendEmailAsync(myMessage);

        }
    }
}

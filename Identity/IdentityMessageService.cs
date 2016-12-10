using Core.Config;
using EmailProvider;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace Identity
{
    public class EmailService : IIdentityMessageService
    {
        private IEmailProvider emailProvider;

        public EmailService(IEmailProvider emailProvider)
        {
            this.emailProvider = emailProvider;
        }

        public Task SendAsync(IdentityMessage message)
        {
            var email = new Email();

            email.From = AppConfig.NoReplyEmailAddress;
            email.To = message.Destination;
            email.Subject = message.Subject;
            email.IsHtml = true;
            email.HtmlBody = message.Body;

            return emailProvider.SendEmailAsync(email);
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0); // not implemented
        }
    }
}

using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using Service.Services.Emails;

namespace Identity
{
    public class IdentityMessageService : IIdentityMessageService
    {
        private readonly IEmailService _emailService;

        public IdentityMessageService(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public Task SendAsync(IdentityMessage message)
        {
            return _emailService.SendEmailAsync(message.Destination, message.Subject, message.Body);
        }
    }

    //public class SmsService : IIdentityMessageService
    //{
    //    public Task SendAsync(IdentityMessage message)
    //    {
    //        // Plug in your SMS service here to send a text message.
    //        return Task.FromResult(0); // not implemented
    //    }
    //}
}

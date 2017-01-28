using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Core.Config;

namespace EmailProvider
{
    public class GoogleEmailProvider : IEmailProvider
    {
        public string GmailUsername => AppConfig.GmailUserName;

        public int GmailPort => AppConfig.GmailPort;

        public string GmailPassword => AppConfig.GmailPassword;

        public string GmailHost => AppConfig.GmailHost;

        public bool GmailSsl => AppConfig.GmailSsl;

        public void SendEmail(IEmailMessage emailMessage)
        {
            var smtp = new SmtpClient
            {
                Host = GmailHost,
                Port = GmailPort,
                EnableSsl = GmailSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(GmailUsername, GmailPassword)
            };

            using (var mailMessage = new MailMessage(GmailUsername, emailMessage.To))
            {
                mailMessage.Subject = emailMessage.Subject;
                mailMessage.Body = emailMessage.HtmlBody;
                mailMessage.IsBodyHtml = emailMessage.IsHtml;
                smtp.Send(mailMessage);
            }
        }

        public async Task SendEmailAsync(IEmailMessage emailMessage)
        {
            var smtp = new SmtpClient
            {
                Host = GmailHost,
                Port = GmailPort,
                EnableSsl = GmailSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(GmailUsername, GmailPassword)
            };

            using (var mailMessage = new MailMessage(GmailUsername, emailMessage.To))
            {
                mailMessage.Subject = emailMessage.Subject;
                mailMessage.Body = emailMessage.HtmlBody;
                mailMessage.IsBodyHtml = emailMessage.IsHtml;
                await smtp.SendMailAsync(mailMessage);
            }
        }
    }
}

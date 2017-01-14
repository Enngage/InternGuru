using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace EmailProvider
{
    public class GoogleEmailProvider : IEmailProvider
    {
        public string GmailUsername => ConfigurationManager.AppSettings["GmailUsername"];

        public int GmailPort => Convert.ToInt32(ConfigurationManager.AppSettings["GmailPort"]);

        public string GmailPassword => ConfigurationManager.AppSettings["GmailPassword"];

        public string GmailHost => ConfigurationManager.AppSettings["GmailHost"];

        public bool GmailSsl => ConfigurationManager.AppSettings["GmailSSL"].Equals("true", StringComparison.OrdinalIgnoreCase);

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
            var smtp = new SmtpClient();
            smtp.Host = GmailHost;
            smtp.Port = GmailPort;
            smtp.EnableSsl = GmailSsl;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(GmailUsername, GmailPassword);

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

using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace EmailProvider
{
    public class GoogleEmailProvider : IEmailProvider
    {
        public string GmailUsername
        {
            get
            {
                return ConfigurationManager.AppSettings["GmailUsername"];
            }
        }

        public int GmailPort
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["GmailPort"]);
            }
        }

        public string GmailPassword
        {
            get
            {
                return ConfigurationManager.AppSettings["GmailPassword"];
            }
        }

        public string GmailHost
        {
            get
            {
                return ConfigurationManager.AppSettings["GmailHost"];
            }
        }

        public bool GmailSSL
        {
            get
            {
                return ConfigurationManager.AppSettings["GmailSSL"].Equals("true", StringComparison.OrdinalIgnoreCase) ? true : false;
            }
        }

        public void SendEmail(IEmail email)
        {
            SmtpClient smtp = new SmtpClient();
            smtp.Host = GmailHost;
            smtp.Port = GmailPort;
            smtp.EnableSsl = GmailSSL;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(GmailUsername, GmailPassword);

            using (var mailMessage = new MailMessage(GmailUsername, email.To))
            {
                mailMessage.Subject = email.Subject;
                mailMessage.Body = email.HtmlBody;
                mailMessage.IsBodyHtml = email.IsHtml;
                smtp.Send(mailMessage);
            }
        }

        public async Task SendEmailAsync(IEmail email)
        {
            SmtpClient smtp = new SmtpClient();
            smtp.Host = GmailHost;
            smtp.Port = GmailPort;
            smtp.EnableSsl = GmailSSL;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(GmailUsername, GmailPassword);

            using (var mailMessage = new MailMessage(GmailUsername, email.To))
            {
                mailMessage.Subject = email.Subject;
                mailMessage.Body = email.HtmlBody;
                mailMessage.IsBodyHtml = email.IsHtml;
                await smtp.SendMailAsync(mailMessage);
            }
        }
    }
}

using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Core.Config;
using EmailProvider;
using Entity;

namespace Service.Services.Emails
{
    public class EmailService : BaseService<Email>, IEmailService
    {
        #region Config

        private readonly string _fromEmailAddress = AppConfig.FromEmailAddress;
        private readonly string _noSubjectText = AppConfig.NoSubjectText;

        #endregion

        public EmailService(IServiceDependencies serviceDependencies) : base(serviceDependencies) { }

        public async Task SendEmailAsync(string recipientEmail, string subject, string text)
        {
            var emailSubject = string.IsNullOrEmpty(subject) ? _noSubjectText : subject;

            // try to send e-mail
            try
            {
                await EmailProvider.SendEmailAsync(new EmailMessage()
                {
                    From = _fromEmailAddress,
                    To = recipientEmail,
                    Subject = emailSubject,
                    HtmlBody = text,
                    IsHtml = true,
                });

                // e-mail sent -> log e-mail success
                var newEmail = new Email()
                {
                    Subject = emailSubject,
                    From = _fromEmailAddress,
                    To = recipientEmail,
                    HtmlBody = text,
                    IsSent = true,
                    Result = "",
                    Sent = DateTime.Now,
                };

                await InsertAsync(newEmail);

            }
            catch (Exception ex)
            {
                // e-mail was not send successfully -> log e-mail error
                var newEmail = new Email()
                {
                    Subject = emailSubject,
                    From = _fromEmailAddress,
                    To = recipientEmail,
                    HtmlBody = text,
                    IsSent = false,
                    Result = ex.Message,
                    Sent = null
                };

                await InsertAsync(newEmail);
            }
        }

        public void SendEmail(string recipientEmail, string subject, string text)
        {
            var emailSubject = string.IsNullOrEmpty(subject) ? _noSubjectText : subject;

            // try to send e-mail
            try
            {

                EmailProvider.SendEmail(new EmailMessage()
                {
                    From = _fromEmailAddress,
                    To = recipientEmail,
                    Subject = emailSubject,
                    HtmlBody = text,
                    IsHtml = true,
                });

                // e-mail sent -> log e-mail success
                var newEmail = new Email()
                {
                    Subject = emailSubject,
                    From = _fromEmailAddress,
                    To = recipientEmail,
                    HtmlBody = text,
                    IsSent = true,
                    Result = "",
                    Sent = DateTime.Now,
                };

                Insert(newEmail);

            }
            catch (Exception ex)
            {
                // e-mail was not send successfully -> log e-mail error
                var newEmail = new Email()
                {
                    Subject = emailSubject,
                    From = _fromEmailAddress,
                    To = recipientEmail,
                    HtmlBody = text,
                    IsSent = false,
                    Result = ex.Message,
                    Sent = null
                };

                Insert(newEmail);
            }
        }

        public IInsertActionResult Insert(Email email)
        {
            // set created date
            email.Created = DateTime.Now;

            // set guid
            email.Guid = Guid.NewGuid();

            return InsertObject(AppContext.Emails, email);
        }

        public void LogFailedEmail(string recipientEmail, string subject, string text, string result)
        {
            var emailSubject = string.IsNullOrEmpty(subject) ? _noSubjectText : subject;

            var newEmail = new Email()
            {
                Subject = emailSubject,
                From = _fromEmailAddress,
                To = recipientEmail,
                HtmlBody = text,
                IsSent = false,
                Result = result,
                Sent = null
            };

            Insert(newEmail);
        }

        public override IDbSet<Email> GetEntitySet()
        {
            return AppContext.Emails;
        }
    }
}

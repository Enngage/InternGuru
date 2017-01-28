using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Core.Config;
using EmailProvider;
using Entity;
using Service.Exceptions;

namespace Service.Services.Emails
{
    public class EmailService : BaseService<Email>, IEmailService
    {
        #region Config

        private readonly string _fromEmailAddress = AppConfig.FromEmailAddress;
        private readonly string _noSubjectText = AppConfig.NoSubjectText;

        #endregion

        public EmailService(IServiceDependencies serviceDependencies) : base(serviceDependencies) { }

        public Task<int> DeleteAsync(int id)
        {
            var email = AppContext.Emails.Find(id);

            if (email != null)
            {
                // delete email
                AppContext.Emails.Remove(email);

                // touch cache keys
                TouchDeleteKeys(email);

                // fire event
                OnDelete(email);

                // save changes
                return AppContext.SaveChangesAsync();
            }

            return Task.FromResult(0);
        }

        public Task<Email> GetAsync(int id)
        {
            return AppContext.Emails.FirstOrDefaultAsync(m => m.ID == id);
        }

        public IQueryable<Email> GetAll()
        {
            return AppContext.Emails;
        }

        public IQueryable<Email> GetSingle(int id)
        {
            return AppContext.Emails.Where(m => m.ID == id).Take(1);
        }

        /// <summary>
        /// Inserts e-mail into database
        /// Does not SEND e-mail
        /// </summary>
        /// <param name="obj">e-mail</param>
        /// <returns></returns>
        public Task<int> InsertAsync(Email obj)
        {
            AppContext.Emails.Add(obj);

            // set created date
            obj.Created = DateTime.Now;

            // set guid
            obj.Guid = Guid.NewGuid();

            // touch cache keys
            TouchInsertKeys(obj);

            // fire event
            OnInsert(obj);

            return SaveChangesAsync();
        }

        public Task<int> UpdateAsync(Email obj)
        {
            var email = AppContext.Emails.Find(obj.ID);

            if (email == null)
            {
                throw new NotFoundException($"Email with ID: {obj.ID} not found");
            }

            // fire event
            OnUpdate(obj, email);

            // set guid
            obj.Guid = email.Guid;

            // update log
            AppContext.Entry(email).CurrentValues.SetValues(obj);

            // touch cache keys
            TouchUpdateKeys(email);

            // save changes
            return AppContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Email>> GetAllCachedAsync()
        {
            return await CacheService.GetOrSetAsync(async () => await GetAll().ToListAsync(), GetCacheAllCacheSetup());
        }

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

        public int Insert(Email email)
        {
            AppContext.Emails.Add(email);

            // set created date
            email.Created = DateTime.Now;

            // set guid
            email.Guid = Guid.NewGuid();

            // touch cache keys
            TouchInsertKeys(email);

            // fire event
            OnInsert(email);

            return SaveChanges();
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
    }
}

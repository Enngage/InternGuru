using System.Threading.Tasks;
using Entity;

namespace Service.Services.Emails
{
    public interface IEmailService : IService<Email>
    {
        /// <summary>
        /// Inserts e-mail synchronously
        /// Does NOT send e-mail
        /// </summary>
        /// <param name="email">E-mail to insert</param>
        /// <returns>ID of the inserted e-mail</returns>
        int Insert(Email email);

        /// <summary>
        /// Attempts to send e-mail and inserts record in database
        /// </summary>
        /// <param name="recipientEmail">E-mail address of recipient</param>
        /// <param name="subject">Subject of e-mail</param>
        /// <param name="text">Text of e-mail (may contain HTML)</param>
        /// <returns></returns>
        Task SendEmailAsync(string recipientEmail, string subject, string text);

        /// <summary>
        /// Attempts to send e-mail and inserts record in database
        /// </summary>
        /// <param name="recipientEmail">E-mail address of recipient</param>
        /// <param name="subject">Subject of e-mail</param>
        /// <param name="text">Text of e-mail (may contain HTML)</param>
        /// <returns></returns>
        void SendEmail(string recipientEmail, string subject, string text);

        /// <summary>
        /// Creates a failed e-mail in database
        /// Does NOT send e-mail
        /// </summary>
        /// <param name="recipientEmail">E-mail address of recipient</param>
        /// <param name="subject">Subject of e-mail</param>
        /// <param name="text">Text of e-mail (may contain HTML)</param>
        /// <param name="result">Error message or other reason why e-mail couldn't be send</param>
        void LogFailedEmail(string recipientEmail, string subject, string text, string result);
    }
}
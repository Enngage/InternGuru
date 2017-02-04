using System;
using System.Linq;
using System.Web;
using Entity;
using UI.Builders.Services;
using UI.Helpers;

namespace UI.Events.EventClasses
{
    public class NotificationEvents
    {
        #region Services

        private IServicesLoader Services { get; }
        private readonly System.Web.Mvc.UrlHelper _urlHelper = new System.Web.Mvc.UrlHelper(HttpContext.Current.Request.RequestContext);

        /// <summary>
        /// Url to the main site without actions/params
        /// </summary>
        private string SiteUrl => _urlHelper.RequestContext.HttpContext.Request.Url?.Scheme + "://" + _urlHelper.RequestContext.HttpContext.Request.Url?.Authority;

        #endregion

        #region Constructor

        public NotificationEvents(IServicesLoader services)
        {
            Services = services;
        }

        #endregion

        #region Notification messages

        /// <summary>
        /// Sends message to the author of internship indicating that internship is active
        /// </summary>
        /// <param name="internshipNew">Newly saved internship</param>
        /// <param name="internshipOriginal">Original internship</param>
        public void SendInternshipActiveNotification(Internship internshipNew, Internship internshipOriginal = null)
        {
            if (internshipNew == null)
            {
                throw new ArgumentNullException(nameof(internshipNew));
            }

            bool sendNotification = false;

            if (internshipOriginal == null)
            {
                sendNotification = internshipNew.IsActive;
            }
            else
            {
                sendNotification = internshipNew.IsActive && !internshipOriginal.IsActive;
            }

            if (sendNotification)
            {

                // sent notification
                // get recipient
                var recipient = Services.IdentityService.GetSingle(internshipNew.ApplicationUserId).FirstOrDefault();

                if (recipient == null)
                {
                    throw new ArgumentNullException($"Nelze odeslat e-mail protože uživatel s ID = {internshipNew.ApplicationUserId} neexistuje");
                }

                var subject = $"Stáž {internshipNew.Title} je aktivní";
                var title = $"{internshipNew.Title} ";
                var message = $"Stáž {internshipNew.Title} byla právě aktivována a je viditelná pro všechny návštěvníky";
                var emailHtml = Services.EmailTemplateService.GetBasicTemplate(recipient.Email, title, message, message, GetInternshipUrl(internshipNew.ID, internshipNew.CodeName), "Zobrazit");

                try
                {
                    // send e-mail
                    this.Services.EmailService.SendEmail(recipient.Email, subject, emailHtml);
                }
                catch (Exception ex)
                {
                    // e-mail could not be send because the template was not found
                    this.Services.EmailService.LogFailedEmail(recipient.Email, subject, message, ex.Message);

                    throw;
                }
            }
        }

        /// <summary>
        /// Sends e-mail notification that a new message was received
        /// </summary>
        /// <param name="message">Message</param>
        public void SendMessageNotificationToRecipient(Message message)
        {
            // get recipient
            var recipient = Services.IdentityService.GetSingle(message.RecipientApplicationUserId).FirstOrDefault();

            if (recipient == null)
            {
                throw new ArgumentNullException($"Nelze odeslat e-mail protože uživatel s ID = {message.RecipientApplicationUserId} neexistuje");
            }

            // get sender
            var sender = Services.IdentityService.GetSingle(message.SenderApplicationUserId).FirstOrDefault();
            if (sender == null)
            {
                throw new ArgumentException($"Nelze odeslat e-mail od uživatele s ID = {message.SenderApplicationUserId} protože neexistuje");
            }

            const int previewCharLength = 120;
            const string subject = "Nová zpráva";
            var title = $"Zpráva od {UserHelper.GetDisplayNameStatic(sender.FirstName, sender.LastName, sender.Nickname, sender.UserName)}";
            var preheader = message.MessageText.Length > previewCharLength
                ? message.MessageText.Substring(0, previewCharLength - 1)
                : message.MessageText;
            var emailHtml = Services.EmailTemplateService.GetBasicTemplate(recipient.Email, title, message.MessageText, preheader, GetConversationUrl(message.SenderApplicationUserId), "Odepsat");

            try
            {
                // send e-mail
                this.Services.EmailService.SendEmail(recipient.Email, subject, emailHtml);
            }
            catch (Exception ex)
            {
                // e-mail could not be send because the template was not found
                var emailText = $"{message.MessageText}";

                this.Services.EmailService.LogFailedEmail(recipient.Email, subject, emailText, ex.Message);

                throw;
            }
        }

        #endregion

        #region Helper methods


        #endregion

        #region Url helper methods

        /// <summary>
        /// Gets url to Conversation page used in e-mail notification
        /// </summary>
        /// <param name="senderApplicationId">ID of the sender user</param>
        /// <returns>URL of the conversation</returns>
        private string GetConversationUrl(string senderApplicationId)
        {
            return SiteUrl + "/" + _urlHelper.Action("Conversation", "Auth", new { id = senderApplicationId });
        }

        /// <summary>
        /// Gets url to internship
        /// </summary>
        /// <param name="internshipID">InternshipID</param>
        /// <param name="internshipCodeName">IntersnhipCodeName</param>
        /// <returns>URL of the internship</returns>
        private string GetInternshipUrl(int internshipID, string internshipCodeName)
        {
            return SiteUrl + "/" + _urlHelper.Action("Index", "Internship", new { id = internshipID, codeName = internshipCodeName });
        }

        #endregion
    }
}

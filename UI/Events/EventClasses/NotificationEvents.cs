using System;
using System.Linq;
using Core.Config;
using Entity;
using UI.Builders.Services;
using UI.Helpers;

namespace UI.Events.EventClasses
{
    public class NotificationEvents
    {
        #region Services

        private IServicesLoader Services { get; set; }

        #endregion

        #region Constructor

        public NotificationEvents(IServicesLoader services)
        {
            Services = services;
        }

        #endregion

        #region Notification messages

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
            var title = $"Zpráva od {UserHelper.GetDisplayName(sender.FirstName, sender.LastName, sender.Nickname, sender.UserName)}";
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
            }
        }

        #endregion

        #region Url helper methods

        /// <summary>
        /// Gets url to Conversation page used in e-mail notification
        /// </summary>
        /// <param name="senderApplicationId">ID of the sender user</param>
        /// <returns>URL of the conversation</returns>
        private static string GetConversationUrl(string senderApplicationId)
        {
            return $"{AppConfig.WebUrl}Auth/Conversation/{senderApplicationId}";
        }

        #endregion
    }
}

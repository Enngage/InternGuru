using Entity;
using System.Data.Entity;
using System.Linq;
using System;
using UI.Builders.Services;
using System.Collections.Generic;
using UI.UIServices.Models;
using EmailProvider;
using Common.Config;

namespace UI.Events
{
    public class NotificationEvents
    {
        #region Config

        private const string NEW_MESSAGE_TO_RECIPIENT_TEMPLATE = "Notification_NewMessageToRecipient.html";

        #endregion

        #region Services

        private IServicesLoader Services { get; set; }

        #endregion

        #region Constructor

        public NotificationEvents(IServicesLoader services)
        {
            Services = services;
        }

        #endregion

        /// <summary>
        /// Sends e-mail notification that a new message was received
        /// </summary>
        /// <param name="message">Message</param>
        public void SendMessageNotifications(Message message)
        {
            // get recipient
            var recipient = Services.IdentityService.GetSingle(message.RecipientApplicationUserId).FirstOrDefault();

            if (recipient == null)
            {
                throw new ArgumentNullException($"Nelze odeslat e-mail protože uživatel s ID {message.RecipientApplicationUserId} neexistuje");
            }

            // get email template
            var replacements = new List<MacroReplacement>()
            {
                new MacroReplacement()
                {
                    MacroName = "RecipientEmail",
                    Value = recipient.Email
                },
                new MacroReplacement()
                {
                    MacroName = "MessageText",
                    Value = message.MessageText
                },
                new MacroReplacement()
                {
                    MacroName = "Subject",
                    Value = message.Subject
                },
            };

            var emailHtmlTemplate = Services.EmailTemplateService.GetTemplateHTML(NEW_MESSAGE_TO_RECIPIENT_TEMPLATE, replacements);

            if (string.IsNullOrEmpty(emailHtmlTemplate))
            {
                throw new ArgumentNullException($"E-mail šablona {NEW_MESSAGE_TO_RECIPIENT_TEMPLATE} je nevalidní");
            }

            // send e-mail
            var email = new Email()
            {
                From = AppConfig.NoReplyEmailAddress,
                HtmlBody = emailHtmlTemplate,
                IsHtml = true,
                Subject = $"Nová zpráva: {message.Subject}",
                To = recipient.Email
            };

            this.Services.EmailProvider.SendEmail(email);
        }
    }
}

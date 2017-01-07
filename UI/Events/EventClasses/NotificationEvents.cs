using System;
using System.Collections.Generic;
using System.Linq;
using Core.Config;
using EmailProvider;
using Entity;
using UI.Builders.Services;
using UI.UIServices.Models;

namespace UI.Events.EventClasses
{
    public class NotificationEvents
    {
        #region Config

        private const string NewMessageToRecipientTemplate = "Notification_NewMessageToRecipient.html";

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

            var emailHtmlTemplate = Services.EmailTemplateService.GetTemplateHtml(NewMessageToRecipientTemplate, replacements);

            if (string.IsNullOrEmpty(emailHtmlTemplate))
            {
                throw new ArgumentNullException($"E-mail šablona {NewMessageToRecipientTemplate} je nevalidní");
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

            Services.EmailProvider.SendEmail(email);
        }
    }
}

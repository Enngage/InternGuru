using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core.Config;
using EmailProvider;
using Entity;
using UI.Builders.Services;
using UI.Emails;
using UI.UIServices.Models;

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

            try
            {
                var emailHtml = Services.EmailTemplateService.GetTemplateHtml(EmailTypeEnum.NotificationNewMessageToRecipient, replacements);

                if (string.IsNullOrEmpty(emailHtml))
                {
                    var error = $"E-mail šablona {EmailTypeEnum.NotificationNewMessageToRecipient} je prázdná";

                    this.Services.EmailService.LogFailedEmail(recipient.Email, $"Nová zpráva: {message.Subject}", string.Join(",", replacements), error);

                    throw new ArgumentNullException(error);
                }

                // send e-mail
                this.Services.EmailService.SendEmail(recipient.Email, $"Nová zpráva: {message.Subject}", emailHtml);
            }
            catch (FileNotFoundException ex)
            {
                // e-mail could not be send because the template was not found
                this.Services.EmailService.LogFailedEmail(recipient.Email, $"Nová zpráva: {message.Subject}", string.Join(",", replacements), ex.Message);
            }
        }
    }

    #endregion
}

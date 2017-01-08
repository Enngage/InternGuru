
using System;
using UI.Builders.InfoMessage.Enums;

namespace UI.Builders.InfoMessage.Views
{
    public class InfoMessageView
    {
        /// <summary>
        /// ID of the message element
        /// </summary>
        public string MessageID { get; set; }

        /// <summary>
        /// Info message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Title of message
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Indicates if message can be closed by user
        /// </summary>
        public bool IsClosable { get; set; }

        /// <summary>
        /// Message type
        /// </summary>
        public InfoMessageTypeEnum MessageType { get; set; }

        /// <summary>
        /// Indicates when CLOSED message will become visible again
        /// </summary>
        public DateTime RememberClosedUntil { get; set; }

        /// <summary>
        /// Indicates if hidden message will be remembered until the configured expiration
        /// </summary>
        public bool RememberClosed => RememberClosedUntil == DateTime.MinValue || DateTime.Now <= RememberClosedUntil;

        /// <summary>
        /// Cookie name used to identify closed messages
        /// </summary>
        public string CookieName => InfoMessageBuilder.GetClosedMessageCookieName(MessageID);

    }
}

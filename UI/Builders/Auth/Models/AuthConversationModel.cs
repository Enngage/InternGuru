using System;
using UI.Helpers;

namespace UI.Builders.Auth.Models
{
    public class AuthConversationModel
    {
        public string MessageText { get; set; }
        public string Subject { get; set; }
        public int ID { get; set; }
        public DateTime MessageCreated { get; set; }
        public string RecipientApplicationUserId { get; set; }
        public string RecipientApplicationUserName { get; set; }
        public string SenderApllicationUserId { get; set; }
        public string RecipientNickname { get; set; }
        public string SenderApplicationUserName { get; set; }
        public string CurrentUserId { get; set; }
        public string RecipientFirstName { get; set; }
        public string RecipientLastName { get; set; }
        public string SenderFirstName { get; set; }
        public string SenderLastName { get; set; }
        public string SenderNickname { get; set; }

        public bool IsRead { get; set; }
      
        /// <summary>
        /// Represents either recipient ID or sender ID
        /// </summary>
        public string ConversationID => WrittenByCurrentUser ? RecipientApplicationUserId : SenderApllicationUserId;

        /// <summary>
        /// Represent the ID of other user (opposite to current user)
        /// </summary>
        public string WithApplicationUserId => WrittenByCurrentUser ? RecipientApplicationUserId : SenderApllicationUserId;

        /// <summary>
        /// Represent the user name of other user (opposite to current user)
        /// </summary>
        public string WithApplicationUserName => WrittenByCurrentUser ? RecipientApplicationUserName : SenderApplicationUserName;

        /// <summary>
        /// Represent the display name of other user (opposite to current user)
        /// </summary>
        public string WithApplicationDisplayName => WrittenByCurrentUser ? UserHelper.GetDisplayName(RecipientFirstName, RecipientLastName, RecipientNickname, RecipientApplicationUserName) : UserHelper.GetDisplayName(SenderFirstName, SenderLastName, SenderNickname, SenderApplicationUserName);

        /// <summary>
        /// Indicates if the message was sent by current user (outgoing message) or incoming
        /// </summary>
        public bool WrittenByCurrentUser => CurrentUserId == SenderApllicationUserId;
    }
}

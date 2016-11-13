using System;

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
        public string SenderApplicationUserName { get; set; }
        public string CurrentUserId { get; set; }
        public string RecipientFirstName { get; set; }
        public string RecipientLastName { get; set; }
        public string SenderFirstName { get; set; }
        public string SenderLastName { get; set; }

        public bool IsRead { get; set; }
      
        /// <summary>
        /// Represents either recipient ID or sender ID
        /// </summary>
        public string ConversationID
        {
            get
            {
                return WrittenByCurrentUser ? RecipientApplicationUserId : SenderApllicationUserId;
            }
        }

        public string WithApplicationUserId
        {
            get
            {
                return WrittenByCurrentUser ? RecipientApplicationUserId : SenderApllicationUserId;
            }
        }
        public string WithApplicationUserName
        {
            get
            {
                return WrittenByCurrentUser ? RecipientApplicationUserName : SenderApplicationUserName;
            }
        }

        /// <summary>
        /// Indicates if the message was sent by current user (outgoing message) or incoming
        /// </summary>
        public bool WrittenByCurrentUser
        {
            get
            {
                return CurrentUserId == SenderApllicationUserId;
            }
        }
    }
}

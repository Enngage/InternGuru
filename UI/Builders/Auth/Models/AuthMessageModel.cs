using System;

namespace UI.Builders.Auth.Models
{
    public class AuthMessageModel
    {
        public string MessageText { get; set; }
        public string Subject { get; set; }
        public int ID { get; set; }
        public DateTime MessageCreated { get; set; }
        public string SenderApllicationUserId { get; set; }
        public string SenderApplicationUserName { get; set; }
        public string RecipientApplicationUserId { get; set; }
        public string RecipientApplicationUserName { get; set; }
        public string RecipientFirstName { get; set; }
        public string RecipientLastName { get; set; }
        public string SenderFirstName { get; set; }
        public string SenderLastName { get; set; }
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string CurrentUserId { get; set; }
        public bool IsRead { get; set; }
        public bool CompanyIsRecipient
        {
            get
            {
                return CompanyID != 0;
            }
        }

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

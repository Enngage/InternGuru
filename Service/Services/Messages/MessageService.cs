using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Entity;
using Service.Events;
using Service.Models;

namespace Service.Services.Messages
{
    public class MessageService : BaseService<Message>, IMessageService
    {

        public MessageService(IServiceDependencies serviceDependencies) : base(serviceDependencies) { }

        public async Task<int> MarkMessagesAsRead(string recipientUserId, string senderUserId)
        {
            var messages = await GetAll()
                .Where(m => (m.RecipientApplicationUserId == recipientUserId && m.SenderApplicationUserId == senderUserId) && m.IsRead == false)
                .ToListAsync();

            foreach (var message in messages)
            {
                // mark message as read
                message.IsRead = true;

                // update
                AppContext.Entry(message).CurrentValues.SetValues(message);

                // touch cache keys
                TouchUpdateKeys(message);
            }

            return await SaveChangesAsync();
        }

        public override ValidationResult ValidateObject(SaveEventType eventType, Message newObj, Message oldObj = null)
        {
            // do not let users send messages to themselves
            if (newObj.SenderApplicationUserId.Equals(newObj.RecipientApplicationUserId, StringComparison.OrdinalIgnoreCase))
            {
                return new ValidationResult()
                {
                    ErrorMessage = "Recipient has to be different then sender",
                    IsValid = false
                };
            }
            return new ValidationResult()
            {
                IsValid = true
            };
        }

        public override IDbSet<Message> GetEntitySet()
        {
            return AppContext.Messages;
        }
    }
}

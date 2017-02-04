using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Entity;
using Service.Exceptions;

namespace Service.Services.Messages
{
    public class MessageService : BaseService<Message>, IMessageService
    {

        public MessageService(IServiceDependencies serviceDependencies) : base(serviceDependencies) { }

        #region IService members

        public override Task<int> InsertAsync(Message obj)
        {
            // do not allow messages to self
            if (obj.SenderApplicationUserId == obj.RecipientApplicationUserId)
            {
                throw new ValidationException($"Nelze odeslat sám sobě");
            }

            obj.MessageCreated = DateTime.Now;

            return base.InsertAsync(obj);
        }

        public override Task<int> UpdateAsync(Message obj)
        {
            var message = AppContext.Messages.Find(obj.ID);

            if (message == null)
            {
                throw new NotFoundException($"Message with ID: {obj.ID} not found");
            }

            // keep the created date
            obj.MessageCreated = message.MessageCreated;

            // save changes
            return base.UpdateAsync(obj, message);
        }

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

        public override IDbSet<Message> GetEntitySet()
        {
            return this.AppContext.Messages;
        }
    }

    #endregion
}

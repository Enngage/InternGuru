using System;
using System.Collections.Generic;
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

        public Task<int> InsertAsync(Message obj)
        {
            // do not allow messages to self
            if (obj.SenderApplicationUserId == obj.RecipientApplicationUserId)
            {
                throw new ValidationException($"Nelze odeslat sám sobě");
            }

            AppContext.Messages.Add(obj);

            obj.MessageCreated = DateTime.Now;

            // touch cache keys
            TouchInsertKeys(obj);

            // fire event
            OnInsert(obj);

            return SaveChangesAsync();
        }

        public Task<int> DeleteAsync(int id)
        {
            var message = AppContext.Messages.Find(id);

            if (message != null)
            {
                AppContext.Messages.Remove(message);

                // touch cache keys
                TouchDeleteKeys(message);

                // fire event
                OnDelete(message);

                // save changes
                return AppContext.SaveChangesAsync();

            }

            return Task.FromResult(0);
        }

        public Task<int> UpdateAsync(Message obj)
        {
            var message = AppContext.Messages.Find(obj.ID);

            if (message == null)
            {
                throw new NotFoundException($"Message with ID: {obj.ID} not found");
            }

            // fire event
            OnUpdate(obj, message);

            // keep the created date
            obj.MessageCreated = message.MessageCreated;

            // update
            AppContext.Entry(message).CurrentValues.SetValues(obj);

            // touch cache keys
            TouchUpdateKeys(message);

            // save changes
            return AppContext.SaveChangesAsync();
        }

        public IQueryable<Message> GetAll()
        {
            return AppContext.Messages;
        }

        public IQueryable<Message> GetSingle(int id)
        {
            return AppContext.Messages.Where(m => m.ID == id).Take(1);
        }

        public Task<Message> GetAsync(int id)
        {
            return AppContext.Messages.FirstOrDefaultAsync(m => m.ID == id);
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

        public async Task<IEnumerable<Message>> GetAllCachedAsync()
        {
            return await CacheService.GetOrSetAsync(async () => await GetAll().ToListAsync(), GetCacheAllCacheSetup());
        }
    }

    #endregion
}

using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;

using Cache;
using Core.Context;
using Core.Exceptions;
using Entity;
using System;
using System.Collections.Generic;

namespace Core.Services
{
    public class MessageService : BaseService<Message>, IMessageService
    {

        public MessageService(IAppContext appContext, ICacheService cacheService) : base(appContext, cacheService) { }

        #region IService members

        public Task<int> InsertAsync(Message obj)
        {
            this.AppContext.Messages.Add(obj);

            obj.MessageCreated = DateTime.Now;

            // touch cache keys
            this.TouchInsertKeys(obj);

            // fire event
            this.OnInsert(obj);

            return this.SaveChangesAsync();
        }

        public Task<int> DeleteAsync(int id)
        {
            var message = this.AppContext.Messages.Find(id);

            if (message != null)
            {
                this.AppContext.Messages.Remove(message);

                // touch cache keys
                this.TouchDeleteKeys(message);

                // fire event
                this.OnDelete(message);

                // save changes
                return this.AppContext.SaveChangesAsync();

            }

            return Task.FromResult(0);
        }

        public Task<int> UpdateAsync(Message obj)
        {
            var message = this.AppContext.Messages.Find(obj.ID);

            if (message == null)
            {
                throw new NotFoundException(string.Format("Message with ID: {0} not found", obj.ID));
            }

            // keep the created date
            obj.MessageCreated = message.MessageCreated;

            // update
            this.AppContext.Entry(message).CurrentValues.SetValues(obj);

            // touch cache keys
            this.TouchUpdateKeys(message);

            // fire event
            this.OnUpdate(message);

            // save changes
            return this.AppContext.SaveChangesAsync();
        }

        public IQueryable<Message> GetAll()
        {
            return this.AppContext.Messages;
        }

        public IQueryable<Message> GetSingle(int id)
        {
            return this.AppContext.Messages.Where(m => m.ID == id).Take(1);
        }

        public Task<Message> GetAsync(int id)
        {
            return this.AppContext.Messages.FirstOrDefaultAsync(m => m.ID == id);
        }

        public async Task<int> MarkMessagesAsRead(string recipientUserId, string senderUserId)
        {
            var messages = await this.GetAll()
                .Where(m => (m.RecipientApplicationUserId == recipientUserId && m.SenderApplicationUserId == senderUserId) && m.IsRead == false)
                .ToListAsync();

            foreach (var message in messages)
            {
                // mark message as read
                message.IsRead = true;

                // update
                this.AppContext.Entry(message).CurrentValues.SetValues(message);

                // touch cache keys
                this.TouchUpdateKeys(message);
            }

            return await SaveChangesAsync();
        }

        public async Task<IEnumerable<Message>> GetAllCachedAsync()
        {
            return await this.CacheService.GetOrSetAsync(async () => await this.GetAll().ToListAsync(), this.GetCacheAllCacheSetup());
        }
    }

    #endregion
}

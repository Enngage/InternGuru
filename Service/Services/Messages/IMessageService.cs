using Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Services
{
    public interface IMessageService : IService<Message>
    {
        /// <summary>
        /// Marks conversation messages as read (should be used when the recipient sees the message)
        /// </summary>
        /// <param name="recipientUserId">ID of the recipient user</param>
        /// <param name="senderUserId">ID of the sender user</param>
        /// <returns>Number of modified rows</returns>
        Task<int> MarkMessagesAsRead(string recipientUserId, string senderUserId);
    }
}
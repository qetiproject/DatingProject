using DatingApp.Api.Helpers;
using DatingApp.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatingApp.Api.Data.Repositories
{
    public interface IMessageRepository
    {
        Task<Message> GetMessage(int id);
        Task<PagedList<Message>> GetMessagesForUser(MessageParams messageParams);
        Task<IEnumerable<Message>> GetMessageThread(int userId, int recipientId);
    }
}

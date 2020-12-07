using DatingApp.Api.Models;
using System.Threading.Tasks;

namespace DatingApp.Api.Data
{
    public interface IBaseRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> saveAll();
        Task<Like> GetLike(int userId, int recipientId);
    }
}

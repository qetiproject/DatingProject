using DatingApp.Api.Helpers;
using DatingApp.Api.Models;
using System.Threading.Tasks;

namespace DatingApp.Api.Data
{
    public interface IBaseRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> saveAll();
        Task<PagedList<User>> GetUsers(UserParams userParams);
        Task<PagedList<User>> GetUsersFiltered(UserParams userParams);
        Task<User> GetUser(int id);
        Task<Photo> GetPhoto(int id);
        Task<Photo> GetmainPhotoForUser(int userId);
        Task<Like> GetLike(int userId, int recipientId);
    }
}

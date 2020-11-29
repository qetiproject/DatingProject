using DatingApp.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatingApp.Api.Data
{
    public interface IdatingRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> saveAll();
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUser(int id);
        Task<Photo> GetPhoto(int id);
        Task<Photo> GetmainPhotoForUser(int userId);
    }
}

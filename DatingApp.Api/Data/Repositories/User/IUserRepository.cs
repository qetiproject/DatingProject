using DatingApp.Api.Helpers;
using DatingApp.Api.Models;
using System.Threading.Tasks;

namespace DatingApp.Api.Data.Repositories
{
    public interface IUserRepository
    {
        Task<PagedList<User>> GetUsers(UserParams userParams);
        Task<PagedList<User>> GetUsersFiltered(UserParams userParams);
        Task<User> GetUser(int id);
    }
}

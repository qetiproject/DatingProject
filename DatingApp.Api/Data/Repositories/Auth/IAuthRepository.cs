using DatingApp.Api.Models;
using System.Threading.Tasks;

namespace DatingApp.Api.Data.Auth
{
    public interface IAuthRepository
    {
        Task<User> Register(User user, string password);
        Task<User> Login(string username, string password);
        Task<bool> UserExists(string username);
    }
}

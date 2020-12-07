using DatingApp.Api.Models;
using System.Threading.Tasks;

namespace DatingApp.Api.Data.Repositories
{
    public interface IPhotoRepository
    {
        Task<Photo> GetPhoto(int id);
        Task<Photo> GetmainPhotoForUser(int userId);
    }
}

using DatingApp.Api.Models;
using DatingApp.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DatingApp.Api.Data
{
    public class BaseRepository : IBaseRepository
    {
        private readonly DataContext _context;
        public BaseRepository(DataContext context)
        {
            _context = context;
        }
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<Like> GetLike(int userId, int recipientId)
        {
            return await _context.Likes.FirstOrDefaultAsync(u => u.LikerId == userId && u.LikeeId == recipientId);
        }

        public async Task<bool> saveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}

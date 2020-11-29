using DatingApp.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Photo> Photos { get; set; }

    }
}

using AutoMapper;
using DatingApp.Api.Data.Auth;
using DatingApp.Api.Data.Base;
using DatingApp.Api.Data.Repositories;
using DatingApp.Api.Helpers;
using DatingApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DatingApp.Api.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services, 
            IConfiguration Configuration
        )
        {
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnectionString"));
            });
            services.Configure<CloudinarySettings>(Configuration.GetSection("CloudinarySettings"));
            services.AddAutoMapper(typeof(BaseRepository).Assembly);
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IBaseRepository, BaseRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IPhotoRepository, PhotoRepository>();

            return services;
        }
    }
}

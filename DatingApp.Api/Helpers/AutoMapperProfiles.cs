using AutoMapper;
using DatingApp.Api.Dtos;
using DatingApp.Api.Models;
using System.Linq;

namespace DatingApp.Api.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UsersDto>()
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src =>src.DateOfBirth.CalculateAge()));
            CreateMap<User, UserDetailDto>()
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.Photos.FirstOrDefault(predicate => predicate.IsMain).Url))
                 .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
            CreateMap<Photo, PhotoDetailDto>();
            CreateMap<UserUpdateDto, User>();
        }
        
    }
}

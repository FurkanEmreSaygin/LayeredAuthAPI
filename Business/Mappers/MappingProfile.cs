
using AutoMapper;
using Business.DTOs;
using Domain.Entities;
using Microsoft.Identity.Client;

namespace Business.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserRegisterDto, User>().ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
            CreateMap<User, UserResponseDto>().ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));
            CreateMap<UserUpdateDto, User>().ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
        }
    }
}
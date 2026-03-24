using AutoMapper;
using E_learning.Core.Entities.Identity;
using E_Learning.Core.Entities.Profiles;
using E_Learning.Service.DTOs.Profiles.Admin;

namespace E_Learning.Service.Mapping
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<AdminProfile, UserResponseDto>()
             .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.AppUser.Id))
             .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
             .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.AppUser.FullName))
             .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.AppUser.Email))
             .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.AppUser.PhoneNumber))
             .ForMember(dest => dest.Role, opt => opt.Ignore());

            // Instructor
            CreateMap<InstructorProfile, UserResponseDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.AppUser.Id))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.AppUser.FullName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.AppUser.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.AppUser.PhoneNumber))
                .ForMember(dest => dest.Role, opt => opt.Ignore());
        }
    }
}
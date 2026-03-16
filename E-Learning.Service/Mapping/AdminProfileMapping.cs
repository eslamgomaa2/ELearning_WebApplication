using E_Learning.Core.Entities.Profiles;
using E_Learning.Service.DTOs.Profiles.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace E_Learning.Service.Mapping
{
    public class AdminProfileMapping : Profile
    {
        public AdminProfileMapping()
        {
            CreateMap<CreateAdminProfileDto, AdminProfile>();
            CreateMap<AdminProfile, AdminProfileResponseDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.AppUser.FullName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.AppUser.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.AppUser.PhoneNumber))
                .ForMember(dest => dest.ProfilePicture, opt => opt.MapFrom(src => src.ProfilePicture));
        }
    }
}

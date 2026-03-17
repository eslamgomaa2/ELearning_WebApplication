using E_Learning.Core.Entities.Profiles;
using E_Learning.Service.DTOs.Profiles.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
namespace E_Learning.Service.Mapping
{
    public class StudentProfileMapping : Profile
    {
        public StudentProfileMapping()
        {
          
            CreateMap<UpdateStudentProfileDto, StudentProfile>();

           
            CreateMap<StudentProfile, StudentProfileResponseDto>()

                .ForMember(dest => dest.profileId,
                           opt => opt.MapFrom(src => src.Id))

                .ForMember(dest => dest.Id,
                           opt => opt.MapFrom(src => src.AppUserId))

                .ForMember(dest => dest.Name,
                           opt => opt.MapFrom(src => src.AppUser.FullName))

                .ForMember(dest => dest.Email,
                           opt => opt.MapFrom(src => src.AppUser.Email))

                .ForMember(dest => dest.PhoneNumber,
                           opt => opt.MapFrom(src => src.AppUser.PhoneNumber))

                .ForMember(dest => dest.MemberSince,
                           opt => opt.MapFrom(src => src.AppUser.MemberSince))
                .ForMember(dest => dest.ProfilePicture, 
                         opt => opt.MapFrom(src => src.ProfilePicture));
        }
    }
    }


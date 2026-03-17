using E_Learning.Core.Entities.Profiles;
using E_Learning.Service.DTOs.Profiles.Instructor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace E_Learning.Service.Mapping
{

    public class InstructorProfileMapping : Profile
    {
        public InstructorProfileMapping()
        {

            CreateMap<UpdateInstructorProfileDto, InstructorProfile>();


            CreateMap<InstructorProfile, InstructorProfileResponseDto>()

                .ForMember(dest => dest.profileId,
                           opt => opt.MapFrom(src => src.Id))

                .ForMember(dest => dest.userId,
                           opt => opt.MapFrom(src => src.AppUserId))

                .ForMember(dest => dest.FullName,
                           opt => opt.MapFrom(src => src.AppUser.FullName))

                .ForMember(dest => dest.Email,
                           opt => opt.MapFrom(src => src.AppUser.Email))

                .ForMember(dest => dest.phoneNumber,
                           opt => opt.MapFrom(src => src.AppUser.PhoneNumber))

                .ForMember(dest => dest.ProfilePicture, opt => opt.MapFrom(src => src.ProfilePicture));
              
        }




    }
    }


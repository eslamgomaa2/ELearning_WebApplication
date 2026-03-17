using AutoMapper;
using E_Learning.Core.Entities.LiveSessions;
using E_Learning.Core.Entities.Profiles;
using E_Learning.Service.DTOs.LiveSessionDto;
using E_Learning.Service.DTOs.Profiles.Student;
using E_Learning.Service.DTOs.Profiles.Instructor;
using E_Learning.Service.DTOs.LiveSessionDto.E_Learning.Service.DTOs.LiveSessionDto;

namespace E_Learning.Service.Mapping
{
    public class LiveSessionMappingProfile : Profile
    {
        public LiveSessionMappingProfile()
        {
            // 1. Instructor Mapping (المدرب)
            CreateMap<InstructorProfile, InstructorProfileResponseDto>()
                .ForMember(dest => dest.profileId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.userId, opt => opt.MapFrom(src => src.AppUserId))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.AppUser.FullName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.AppUser.Email))
                .ForMember(dest => dest.phoneNumber, opt => opt.MapFrom(src => src.AppUser.PhoneNumber))
                .ForMember(dest => dest.Bio, opt => opt.MapFrom(src => src.Bio))
                .ForMember(dest => dest.ProfilePictureUrl, opt => opt.MapFrom(src => src.ProfilePicture));

            // 2. Student Mapping (الطالب)
            CreateMap<StudentProfile, StudentProfileResponseDto>()
                .ForMember(dest => dest.profileId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.AppUserId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.AppUser.FullName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.AppUser.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.AppUser.PhoneNumber))
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location))
                .ForMember(dest => dest.ProfilePicture, opt => opt.MapFrom(src => src.ProfilePicture))
                .ForMember(dest => dest.MemberSince, opt => opt.MapFrom(src => src.AppUser.MemberSince)) // جلب تاريخ العضوية من اليوزر
                .ForMember(dest => dest.EngagementRate, opt => opt.MapFrom(src => src.EngagementRate))
                .ForMember(dest => dest.LevelId, opt => opt.MapFrom(src => src.LevelId));

            // 3. LiveSession Mapping (الجلسة المباشرة)
            CreateMap<LiveSession, LiveSessionResponseDto>()
                .ForMember(dest => dest.CourseTitle, opt => opt.MapFrom(src => src.Course != null ? src.Course.Title : string.Empty))
                .ForMember(dest => dest.Instructor, opt => opt.MapFrom(src => src.Instructor)) // يربط الـ Object كامل
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.AttendeesCount, opt => opt.MapFrom(src => src.Attendees != null ? src.Attendees.Count : 0));

            // 4. Attendee Mapping (الحضور)
            CreateMap<LiveSessionAttendee, AttendeeResponseDto>()
                .ForMember(dest => dest.SessionId, opt => opt.MapFrom(src => src.SessionId))
                .ForMember(dest => dest.JoinedAt, opt => opt.MapFrom(src => src.JoinedAt))
                .ForPath(dest => dest.Student.Name, opt => opt.MapFrom(src => src.Student.FullName))
                .ForPath(dest => dest.Student.Email, opt => opt.MapFrom(src => src.Student.Email))
                .ForPath(dest => dest.Student.Id, opt => opt.MapFrom(src => src.Student.Id))
                .ForPath(dest => dest.Student.PhoneNumber, opt => opt.MapFrom(src => src.Student.PhoneNumber));
            // 5. Input Mappings (المدخلات)
            CreateMap<CreateLiveSessionDto, LiveSession>();
            CreateMap<UpdateLiveSessionDto, LiveSession>();
            CreateMap<LogAttendanceDto, LiveSessionAttendee>();
        }
    }
}
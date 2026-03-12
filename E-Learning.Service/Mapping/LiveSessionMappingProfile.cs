using AutoMapper;
using E_Learning.Core.Entities.LiveSessions;
using E_Learning.Core.Enums;
using E_Learning.Service.DTOs.LiveSessionDto;

namespace E_Learning.Service.Mapping
{
    public class LiveSessionMappingProfile : Profile
    {
    
      public LiveSessionMappingProfile()
        {
            // 1. LiveSession → LiveSessionResponseDto
            CreateMap<LiveSession, LiveSessionResponseDto>()
                .ForMember(dest => dest.CourseTitle, opt => opt.MapFrom(src => src.Course != null ? src.Course.Title : string.Empty))
                .ForMember(dest => dest.InstructorName, opt => opt.MapFrom(src => src.Instructor != null ? src.Instructor.UserName : string.Empty))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.AttendeesCount, opt => opt.MapFrom(src => src.Attendees != null ? src.Attendees.Count : 0));

            // 2. LiveSessionAttendee → AttendeeResponseDto
            CreateMap<LiveSessionAttendee, AttendeeResponseDto>()
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student != null ? src.Student.UserName : string.Empty))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Student != null ? src.Student.Email : string.Empty));

            // 3. DTOs → Entities (الإضافة والتعديل)
            CreateMap<CreateLiveSessionDto, LiveSession>();
            CreateMap<UpdateLiveSessionDto, LiveSession>();
            CreateMap<LogAttendanceDto, LiveSessionAttendee>();
        }
    }
}
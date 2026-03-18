using AutoMapper;
using E_Learning.Core.Entities.LiveSessions;
using E_learning.Core.Entities.Identity; // تأكدي من المسار الصحيح لـ ApplicationUser
using E_Learning.Core.Entities.Profiles;
using E_Learning.Service.DTOs.LiveSessionDto;
using E_Learning.Service.DTOs.Profiles.Student;
using E_Learning.Service.DTOs.Profiles.Instructor;
using E_Learning.Service.DTOs.Enrollments.Enrollment;
using E_Learning.Core.Entities.Courses;
using E_Learning.Core.Enums;

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
                .ForMember(dest => dest.Bio, opt => opt.MapFrom(src => src.Bio));
                //.ForMember(dest => dest.ProfilePictureUrl, opt => opt.MapFrom(src => src.ProfilePicture));

            CreateMap<ApplicationUser, InstructorResponseDto>()
.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
.ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
.ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
.ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
.ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
.ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));


            // ── ApplicationUser → StudentSummaryDto ──────────────────────────
            CreateMap<ApplicationUser, StudentSummaryDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.MemberSince, opt => opt.MapFrom(src => src.MemberSince))
                .ForMember(dest => dest.Language, opt => opt.MapFrom(src => src.Language))
                .ForMember(dest => dest.TimeZone, opt => opt.MapFrom(src => src.TimeZone))
                .ForMember(dest => dest.ProfileVisibility, opt => opt.MapFrom(src => src.ProfileVisibility))
                .ForMember(dest => dest.ShowProgressToOthers, opt => opt.MapFrom(src => src.ShowProgressToOthers));

            // ── Course → CourseSummaryDto ────────────────────────────────────
            CreateMap<Course, CourseSummaryDto>()
           .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
           .ForMember(dest => dest.InstructorId, opt => opt.MapFrom(src => src.InstructorId))
           .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
           .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => src.Slug))
           .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
           .ForMember(dest => dest.WhatYouWillLearn, opt => opt.MapFrom(src => src.WhatYouWillLearn))
           .ForMember(dest => dest.ThumbnailUrl, opt => opt.MapFrom(src => src.ThumbnailUrl))
           .ForMember(dest => dest.Language, opt => opt.MapFrom(src => src.Language))
           .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
           .ForMember(dest => dest.DurationInMinutes, opt => opt.MapFrom(src => src.DurationInMinutes))
           .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
           .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
           .ForMember(dest => dest.ApprovedAt, opt => opt.MapFrom(src => src.ApprovedAt));


            // 3. LiveSession Mapping (الجلسة المباشرة) - النمط الكامل
            CreateMap<LiveSession, LiveSessionResponseDto>()
           .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
           .ForMember(dest => dest.InstructorId, opt => opt.MapFrom(src => src.InstructorId))
           .ForMember(dest => dest.Instructor, opt => opt.MapFrom(src => src.Instructor))
           .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
           .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
           .ForMember(dest => dest.ScheduledAt, opt => opt.MapFrom(src => src.ScheduledAt))
           .ForMember(dest => dest.DurationMinutes, opt => opt.MapFrom(src => src.DurationMinutes))
           .ForMember(dest => dest.RoomName, opt => opt.MapFrom(src => src.RoomName))
           .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
           .ForMember(dest => dest.CourseId, opt => opt.MapFrom(src => src.CourseId))
             .ForMember(dest => dest.Course, opt => opt.MapFrom(src => src.Course))
           .ForMember(dest => dest.CourseTitle, opt => opt.MapFrom(src => src.Course != null ? src.Course.Title : string.Empty));

            // 4. Attendee Mapping (الحضور)
            CreateMap<LiveSessionAttendee, AttendeeResponseDto>()
           .ForMember(dest => dest.SessionId, opt => opt.MapFrom(src => src.SessionId))
            .ForMember(dest => dest.LiveSession, opt => opt.MapFrom(src => src.Session))
   .ForMember(dest => dest.StudentId, opt => opt.MapFrom(src => src.Student.Id))
   .ForMember(dest => dest.Student, opt => opt.MapFrom(src => src.Student))
   .ForMember(dest => dest.DurationSeconds, opt => opt.MapFrom(src => src.DurationSeconds))
   .ForMember(dest => dest.JoinedAt, opt => opt.MapFrom(src => src.JoinedAt))
   .ForMember(dest => dest.LeftAt, opt => opt.MapFrom(src => src.LeftAt));

            // 5. Input Mappings (المدخلات)
            CreateMap<UpdateLiveSessionDto, LiveSession>()
    .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));
   
            CreateMap<CreateLiveSessionDto, LiveSession>();
            CreateMap<UpdateLiveSessionDto, LiveSession>();
            CreateMap<LogAttendanceDto, LiveSessionAttendee>();
       }
    }
}
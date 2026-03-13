using AutoMapper;
using E_learning.Core.Entities.Identity;
using E_Learning.Core.Entities.Billing;
using E_Learning.Core.Entities.Courses;
using E_Learning.Core.Entities.Enrollment;
using E_Learning.Service.DTOs.Enrollments.Enrollment;
using E_Learning.Service.DTOs.Enrollments.LessonProgress;

namespace E_Learning.Service.Mapping
{
    public class EnrollmentMappingProfile : Profile
    {
        public EnrollmentMappingProfile()
        {
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

            // ── PaymentTransaction → TransactionSummaryDto ───────────────────
            CreateMap<PaymentTransaction, TransactionSummaryDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.StudentId, opt => opt.MapFrom(src => src.StudentId))
                .ForMember(dest => dest.CourseId, opt => opt.MapFrom(src => src.CourseId))
                .ForMember(dest => dest.PaymentMethodId, opt => opt.MapFrom(src => src.PaymentMethodId))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Currency))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.GatewayReference, opt => opt.MapFrom(src => src.GatewayReference))
                .ForMember(dest => dest.FailureReason, opt => opt.MapFrom(src => src.FailureReason))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.CompletedAt, opt => opt.MapFrom(src => src.CompletedAt));
            CreateMap<Lesson, LessonSummaryDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title));

            // ── LessonProgress → LessonProgressSummaryDto ────────────────────
            // Used inside EnrollmentResponseDto.LessonProgresses list
            CreateMap<LessonProgress, LessonProgressSummaryDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.LessonId, opt => opt.MapFrom(src => src.LessonId))
                .ForMember(dest => dest.Lesson, opt => opt.MapFrom(src => src.Lesson))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.WatchedSeconds, opt => opt.MapFrom(src => src.WatchedSeconds))
                .ForMember(dest => dest.CompletedAt, opt => opt.MapFrom(src => src.CompletedAt))
                .ForMember(dest => dest.LastAccessedAt, opt => opt.MapFrom(src => src.LastAccessedAt));

            // ── Enrollment → EnrollmentSummaryDto ────────────────────────────
            // Lightweight version used inside LessonProgressResponseDto
            // Does NOT include LessonProgresses to avoid circular reference
            CreateMap<Enrollment, EnrollmentSummaryDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.StudentId, opt => opt.MapFrom(src => src.StudentId))
                .ForMember(dest => dest.Student, opt => opt.MapFrom(src => src.Student))
                .ForMember(dest => dest.CourseId, opt => opt.MapFrom(src => src.CourseId))
                .ForMember(dest => dest.Course, opt => opt.MapFrom(src => src.Course))
                .ForMember(dest => dest.TransactionId, opt => opt.MapFrom(src => src.TransactionId))
                .ForMember(dest => dest.Transaction, opt => opt.MapFrom(src => src.Transaction))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.ProgressPercentage, opt => opt.MapFrom(src => src.ProgressPercentage))
                .ForMember(dest => dest.EnrolledAt, opt => opt.MapFrom(src => src.EnrolledAt))
                .ForMember(dest => dest.CompletedAt, opt => opt.MapFrom(src => src.CompletedAt))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted));

            // ── Enrollment → EnrollmentResponseDto ───────────────────────────
            // Full version — includes Student, Course, Transaction, and all LessonProgresses
            CreateMap<Enrollment, EnrollmentResponseDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.StudentId, opt => opt.MapFrom(src => src.StudentId))
                .ForMember(dest => dest.Student, opt => opt.MapFrom(src => src.Student))
                .ForMember(dest => dest.CourseId, opt => opt.MapFrom(src => src.CourseId))
                .ForMember(dest => dest.Course, opt => opt.MapFrom(src => src.Course))
                .ForMember(dest => dest.TransactionId, opt => opt.MapFrom(src => src.TransactionId))
                .ForMember(dest => dest.Transaction, opt => opt.MapFrom(src => src.Transaction))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.ProgressPercentage, opt => opt.MapFrom(src => src.ProgressPercentage))
                .ForMember(dest => dest.EnrolledAt, opt => opt.MapFrom(src => src.EnrolledAt))
                .ForMember(dest => dest.CompletedAt, opt => opt.MapFrom(src => src.CompletedAt))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted));

            // ── LessonProgress → LessonProgressResponseDto ───────────────────
            // Full version — includes Lesson + full Enrollment (with Student + Course)
            CreateMap<LessonProgress, LessonProgressResponseDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.EnrollmentId, opt => opt.MapFrom(src => src.EnrollmentId))
                .ForMember(dest => dest.Enrollment, opt => opt.MapFrom(src => src.Enrollment))
                .ForMember(dest => dest.LessonId, opt => opt.MapFrom(src => src.LessonId))
                .ForMember(dest => dest.Lesson, opt => opt.MapFrom(src => src.Lesson))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.WatchedSeconds, opt => opt.MapFrom(src => src.WatchedSeconds))
                .ForMember(dest => dest.CompletedAt, opt => opt.MapFrom(src => src.CompletedAt))
                .ForMember(dest => dest.LastAccessedAt, opt => opt.MapFrom(src => src.LastAccessedAt));
        }
    }
}
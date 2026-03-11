using AutoMapper;
using E_Learning.Core.Entities.Enrollment;
using E_Learning.Service.DTOs.Enrollments.Enrollment;
using E_Learning.Service.DTOs.Enrollments.LessonProgress;

namespace E_Learning.Service.Mapping
{
    public class EnrollmentMappingProfile : Profile
    {
        public EnrollmentMappingProfile()
        {
            // Enrollment → EnrollmentResponseDto
            CreateMap<Enrollment, EnrollmentResponseDto>()
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student != null ? src.Student.UserName : string.Empty))
                .ForMember(dest => dest.CourseTitle, opt => opt.MapFrom(src => src.Course != null ? src.Course.Title : string.Empty));

            // LessonProgress → LessonProgressResponseDto
            CreateMap<LessonProgress, LessonProgressResponseDto>()
                .ForMember(dest => dest.LessonTitle, opt => opt.MapFrom(src => src.Lesson != null ? src.Lesson.Title : string.Empty));
        }
    }
}
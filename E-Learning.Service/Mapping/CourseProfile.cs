using AutoMapper;
using E_Learning.Core.Entities.Courses;
using E_Learning.Service.DTOs.Course;
using E_Learning.Service.DTOs.CourseDto;
using E_Learning.Service.DTOs.Lesson;
using E_Learning.Service.DTOs.Section;



namespace E_Learning.Service.Mapping
{
    public class CourseProfile :Profile
    {
        public CourseProfile()
        {
            //courses
            CreateMap<Course, CourseDto>()
                .ForMember(dest => dest.InstructorName,
                    opt => opt.MapFrom(src => src.Instructor.UserName))
                .ForMember(dest => dest.LevelName,
                    opt => opt.MapFrom(src => src.Level!.Name));

            CreateMap<CreateCourseDto, Course>();
            CreateMap<UpdateCourseDto, Course>();

            // Section
            CreateMap<Section, SectionDto>();
            CreateMap<CreateSectionDto, Section>();
            CreateMap<UpdateSectionDto, Section>();

            // Lesson
            CreateMap<Lesson, LessonDto>();
            CreateMap<CreateLessonDto, Lesson>();
            CreateMap<UpdateLessonDto, Lesson>();
        }
    }
}

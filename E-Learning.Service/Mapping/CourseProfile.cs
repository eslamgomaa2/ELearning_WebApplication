using AutoMapper;
using E_Learning.Core.Entities.Courses;
using E_Learning.Service.DTOs.CourseDto;
using E_Learning.Service.DTOs.LessonDto;
using E_Learning.Service.DTOs.SectionDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



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

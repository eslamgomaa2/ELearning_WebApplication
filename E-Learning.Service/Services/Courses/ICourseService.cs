using E_Learning.Core.Base;
using E_Learning.Core.Entities.Courses;
using E_Learning.Service.DTOs.Course;
using E_Learning.Service.DTOs.CourseDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Core.Interfaces.Services.Courses
{
    public interface ICourseService
    {
        Task<Response<IReadOnlyList<CourseDto>>> GetCoursesAsync();
        Task<Response<CourseDto>> GetCourseByIdAsync(int id);

        Task<Response<CourseDto>> CreateCourseAsync(CreateCourseDto course);
        Task<Response<string>> UpdateCourseAsync(UpdateCourseDto course);
        Task<Response<string>> DeleteCourseAsync(int id);
    }
}

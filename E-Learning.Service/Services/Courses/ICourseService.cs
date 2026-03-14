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
        Task<Response<IReadOnlyList<CourseDto>>> GetCoursesAsync(CancellationToken ct = default);
        Task<Response<CourseDto>> GetCourseByIdAsync(int id, CancellationToken ct = default);

        Task<Response<CourseDto>> CreateCourseAsync(CreateCourseDto course, CancellationToken ct = default);
        Task<Response<string>> UpdateCourseAsync(UpdateCourseDto course, CancellationToken ct = default);
        Task<Response<string>> DeleteCourseAsync(int id, CancellationToken ct = default);
    }
}

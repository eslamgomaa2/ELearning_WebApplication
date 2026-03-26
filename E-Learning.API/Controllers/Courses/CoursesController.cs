using E_Learning.Core.Features.Courses.Queries;
using E_Learning.Core.Interfaces.Services.Courses;
using E_Learning.Service.DTOs.CourseDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Learning.API.Controllers.Courses
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;
        public CoursesController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCoursesForAdmin([FromQuery] CourseQuery query,
            CancellationToken ct = default)
        {
            var result = await _courseService.GetCoursesAsync(query, ct);
            return Ok(result);
        }

        //[HttpGet()]
        //public async Task<IActionResult> GetCoursesForAdmin(CancellationToken ct = default)
        //{
        //    var result = await _courseService.GetCoursesAsync(ct);
        //    return Ok(result);
        //}

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourse(int id)
        {
            var result = await _courseService.GetCourseByIdAsync(id);
            return StatusCode((int)result.HttpStatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourse(CreateCourseDto dto)
        {
            var result = await _courseService.CreateCourseAsync(dto);
            return StatusCode((int)result.HttpStatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(int id,UpdateCourseDto dto)
        {
            var result = await _courseService.UpdateCourseAsync(id,dto);
            return StatusCode((int)result.HttpStatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var result = await _courseService.DeleteCourseAsync(id);
            return StatusCode((int)result.HttpStatusCode, result);

        }
    }

    
}

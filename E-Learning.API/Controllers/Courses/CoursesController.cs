using E_Learning.Core.Interfaces.Services.Courses;
using E_Learning.Service.DTOs.CourseDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> GetCourses()
        {
            var result = await _courseService.GetCoursesAsync();
            return StatusCode((int)result.HttpStatusCode, result);
        }

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

        [HttpPut]
        public async Task<IActionResult> UpdateCourse(UpdateCourseDto dto)
        {
            var result = await _courseService.UpdateCourseAsync(dto);
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

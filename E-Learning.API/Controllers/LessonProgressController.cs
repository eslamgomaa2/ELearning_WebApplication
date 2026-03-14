using E_Learning.Core.Interfaces.Services.Enrollments;
using E_Learning.Service.DTOs.Enrollments.LessonProgress;
using Microsoft.AspNetCore.Mvc;

namespace E_Learning.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LessonProgressController : ControllerBase
    {
        private readonly ILessonProgressService _lessonProgressService;

        public LessonProgressController(ILessonProgressService lessonProgressService)
        {
            _lessonProgressService = lessonProgressService;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var response = await _lessonProgressService.GetByIdAsync(id, ct);
            return StatusCode((int)response.HttpStatusCode, response);
        }

        [HttpGet("SearchByEnrollment/{enrollmentId:int}")]
        public async Task<IActionResult> GetByEnrollment(int enrollmentId, CancellationToken ct)
        {
            var response = await _lessonProgressService.GetByEnrollmentIdAsync(enrollmentId, ct);
            return StatusCode((int)response.HttpStatusCode, response);
        }

        [HttpGet("SearchByEnrollment/{enrollmentId:int}/SearchByLesson/{lessonId:int}")]
        public async Task<IActionResult> GetByEnrollmentAndLesson(int enrollmentId, int lessonId, CancellationToken ct)
        {
            var response = await _lessonProgressService.GetByEnrollmentAndLessonAsync(enrollmentId, lessonId, ct);
            return StatusCode((int)response.HttpStatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateLessonProgressDto dto, CancellationToken ct)
        {
            var response = await _lessonProgressService.CreateAsync(dto, ct);
            return StatusCode((int)response.HttpStatusCode, response);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateLessonProgressDto dto, CancellationToken ct)
        {
            var response = await _lessonProgressService.UpdateAsync(id, dto, ct);
            return StatusCode((int)response.HttpStatusCode, response);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var response = await _lessonProgressService.DeleteAsync(id, ct);
            return StatusCode((int)response.HttpStatusCode, response);
        }
    }
}
using E_Learning.Core.Interfaces.Services.Enrollments;
using E_Learning.Service.DTOs.Enrollments.Enrollment;
using Microsoft.AspNetCore.Mvc;

namespace E_Learning.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnrollmentsController : ControllerBase
    {
        private readonly IEnrollmentService _enrollmentService;

        public EnrollmentsController(IEnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var response = await _enrollmentService.GetAllAsync(ct);
            return StatusCode((int)response.HttpStatusCode, response);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var response = await _enrollmentService.GetByIdAsync(id, ct);
            return StatusCode((int)response.HttpStatusCode, response);
        }

        [HttpGet("SearchByStudent/{studentId:guid}")]
        public async Task<IActionResult> GetByStudent(Guid studentId, CancellationToken ct)
        {
            var response = await _enrollmentService.GetByStudentIdAsync(studentId, ct);
            return StatusCode((int)response.HttpStatusCode, response);
        }

        [HttpGet("SearchByCourse/{courseId:int}")]
        public async Task<IActionResult> GetByCourse(int courseId, CancellationToken ct)
        {
            var response = await _enrollmentService.GetByCourseIdAsync(courseId, ct);
            return StatusCode((int)response.HttpStatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEnrollmentDto dto, CancellationToken ct)
        {
            var response = await _enrollmentService.CreateAsync(dto, ct);
            return StatusCode((int)response.HttpStatusCode, response);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateEnrollmentDto dto, CancellationToken ct)
        {
            var response = await _enrollmentService.UpdateAsync(id, dto, ct);
            return StatusCode((int)response.HttpStatusCode, response);
        }

        // DELETE /api/enrollments/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var deletedBy = User.Identity?.Name ?? "system";
            var response = await _enrollmentService.SoftDeleteAsync(id, deletedBy, ct);
            return StatusCode((int)response.HttpStatusCode, response);
        }
    }
}
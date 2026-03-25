using E_Learning.Service.Services.ExamServices.Attempts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Learning.API.Controllers
{
    [ApiController]
    [Route("api/exams/{examId:int}/attempts")]
    //[Authorize]
    public class ExamAttemptController : ControllerBase
    {
        private readonly IExamAttemptServices _attemptServices;

        public ExamAttemptController(IExamAttemptServices attemptServices)
        {
            _attemptServices = attemptServices;
        }

        // Helper — extract student Guid from JWT
        private Guid GetStudentId() =>
            Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        

        // ─────────────────────────────────────────────
        // POST api/exams/{examId}/attempts
        // Student starts a new attempt
        // ─────────────────────────────────────────────
        [HttpPost("Start")]
        //[Authorize(Roles = "Student")]
        public async Task<IActionResult> Start(int examId,CancellationToken ct)
        {
            var studentId = Guid.Parse("8e43f608-bccc-49c6-f4fe-08de81f92ce8");

            var result = await _attemptServices.StartAsync(examId, studentId, ct);
            return StatusCode((int)result.HttpStatusCode, result);
        }

        // ─────────────────────────────────────────────
        // GET api/exams/{examId}/attempts/my
        // ⚠️ must be above {attemptId:int} route
        // ─────────────────────────────────────────────
        [HttpGet("my")]
        //[Authorize(Roles = "Student")]
        public async Task<IActionResult> GetMyAttempts(
            int examId,
            CancellationToken ct)
        {
            var result = await _attemptServices.GetMyAttemptsAsync(examId, GetStudentId(), ct);
            return StatusCode((int)result.HttpStatusCode, result);
        }

        // ─────────────────────────────────────────────
        // GET api/exams/{examId}/attempts
        // Instructor sees all attempts
        // ─────────────────────────────────────────────
        [HttpGet]
        //[Authorize(Roles = "Instructor,Admin")]
        public async Task<IActionResult> GetAll( int examId,PaginationParams paginationParams,CancellationToken ct)
        {
            var result = await _attemptServices.GetAllByExamAsync(examId,paginationParams, ct);
            return StatusCode((int)result.HttpStatusCode, result);
        }

        // ─────────────────────────────────────────────
        // GET api/exams/{examId}/attempts/{attemptId}
        // ─────────────────────────────────────────────
        [HttpGet("{attemptId:int}")]
        public async Task<IActionResult> GetById(
            int examId,
            int attemptId,
            CancellationToken ct)
        {
            var result = await _attemptServices.GetByIdAsync(examId, attemptId, ct);
            return StatusCode((int)result.HttpStatusCode, result);
        }

        // ─────────────────────────────────────────────
        // POST api/exams/{examId}/attempts/{attemptId}/submit
        // ─────────────────────────────────────────────
        [HttpPost("{attemptId:int}/submit")]
      //  [Authorize(Roles = "Student")]
        public async Task<IActionResult> Submit(int examId,int attemptId,[FromBody] SubmitAttemptDto dto,CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _attemptServices
                .SubmitAsync(examId, attemptId, GetStudentId(), dto, ct);

            return StatusCode((int)result.HttpStatusCode, result);
        }

        // ─────────────────────────────────────────────
        // PATCH api/exams/{examId}/attempts/{attemptId}/review
        // ─────────────────────────────────────────────
        [HttpPatch("{attemptId:int}/review")]
       // [Authorize(Roles = "Instructor,Admin")]
        public async Task<IActionResult> Review(int examId,int attemptId,[FromBody] ReviewAttemptDto dto,CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _attemptServices
                .ReviewAsync(examId, attemptId, GetStudentId(), dto, ct);

            return StatusCode((int)result.HttpStatusCode, result);
        }

        // ─────────────────────────────────────────────
        // PATCH api/exams/{examId}/attempts/{attemptId}/publish
        // ─────────────────────────────────────────────
        [HttpPatch("{attemptId:int}/publish")]
      //  [Authorize(Roles = "Instructor,Admin")]
        public async Task<IActionResult> Publish(int examId,int attemptId,CancellationToken ct)
        {
            var result = await _attemptServices.PublishAsync(examId, attemptId, ct);
            return StatusCode((int)result.HttpStatusCode, result);
        }
    }
}
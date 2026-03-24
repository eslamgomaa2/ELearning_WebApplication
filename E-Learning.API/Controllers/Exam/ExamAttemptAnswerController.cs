using E_Learning.Service.Services.ExamServices.Answers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Learning.API.Controllers
{
    [ApiController]
    [Route("api/exams/{examId:int}/attempts/{attemptId:int}/answers")]
   // [Authorize]
    public class ExamAttemptAnswerController : ControllerBase
    {
        private readonly IExamAttemptAnswerServices _answerServices;

        public ExamAttemptAnswerController(IExamAttemptAnswerServices answerServices)
        {
            _answerServices = answerServices;
        }

        // ─────────────────────────────────────────────
        // GET api/exams/{examId}/attempts/{attemptId}/answers/text
        // ⚠️ must be above {answerId:int} to avoid routing conflict
        // ─────────────────────────────────────────────
        [HttpGet("text")]
       // [Authorize(Roles = "Instructor,Admin")]
        public async Task<IActionResult> GetTextAnswers(
            int examId,
            int attemptId,
            CancellationToken ct)
        {
            var result = await _answerServices.GetTextAnswersAsync(examId, attemptId, ct);
            return StatusCode((int)result.HttpStatusCode, result);
        }

        // ─────────────────────────────────────────────
        // PATCH api/exams/{examId}/attempts/{attemptId}/answers/grade-all
        // ⚠️ must be above {answerId:int} to avoid routing conflict
        // ─────────────────────────────────────────────
        [HttpPatch("grade-all")]
       // [Authorize(Roles = "Instructor,Admin")]
        public async Task<IActionResult> BulkGrade(
            int examId,
            int attemptId,
            [FromBody] BulkUpdateAnswerScoreDto dto,
            CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _answerServices.BulkGradeAsync(examId, attemptId, dto, ct);
            return StatusCode((int)result.HttpStatusCode, result);
        }

        // ─────────────────────────────────────────────
        // GET api/exams/{examId}/attempts/{attemptId}/answers
        // ─────────────────────────────────────────────
        [HttpGet]
        public async Task<IActionResult> GetAll(
            int examId,
            int attemptId,
            CancellationToken ct)
        {
            var result = await _answerServices.GetByAttemptAsync(examId, attemptId, ct);
            return StatusCode((int)result.HttpStatusCode, result);
        }

        // ─────────────────────────────────────────────
        // GET api/exams/{examId}/attempts/{attemptId}/answers/{answerId}
        // ─────────────────────────────────────────────
        [HttpGet("{answerId:int}")]
        public async Task<IActionResult> GetById(
            int examId,
            int attemptId,
            int answerId,
            CancellationToken ct)
        {
            var result = await _answerServices.GetByIdAsync(examId, attemptId, answerId, ct);
            return StatusCode((int)result.HttpStatusCode, result);
        }

        // ─────────────────────────────────────────────
        // PATCH api/exams/{examId}/attempts/{attemptId}/answers/{questionId}/grade
        // ─────────────────────────────────────────────
        [HttpPatch("{questionId:int}/grade")]
        [Authorize(Roles = "Instructor,Admin")]
        public async Task<IActionResult> GradeAnswer(
            int examId,
            int attemptId,
            int questionId,
            [FromBody] UpdateAnswerScoreDto dto,
            CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _answerServices
                .GradeAnswerAsync(examId, attemptId, questionId, dto, ct);

            return StatusCode((int)result.HttpStatusCode, result);
        }
    }
}
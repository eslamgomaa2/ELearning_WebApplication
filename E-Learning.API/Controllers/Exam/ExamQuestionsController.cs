using E_Learning.Service.Services.ExamServices.Questions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Learning.API.Controllers
{
    [ApiController]
    [Route("api/exams/{examId:int}/questions")]
   // [Authorize(Roles = "Instructor,Admin")]
    public class ExamQuestionController : ControllerBase
    {
        private readonly IExamQuestionServices _questionServices;

        public ExamQuestionController(IExamQuestionServices questionServices)
        {
            _questionServices = questionServices;
        }

        
        [HttpPost]
        public async Task<IActionResult> AddManually( int examId, [FromBody] AddQuestionsDto dto, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _questionServices.AddManuallyAsync(examId, dto, ct);
            return StatusCode((int)result.HttpStatusCode, result);
        }

        
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetByExam(int examId,  CancellationToken ct)
        {
            var result = await _questionServices.GetQuestionsByExamIdAsync(examId, ct);
            return StatusCode((int)result.HttpStatusCode, result);
        }

       
        [HttpPut("reorder")]
        public async Task<IActionResult> Reorder(
            int examId,
            [FromBody] ReorderQuestionsDto dto,
            CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _questionServices.ReorderAsync(examId, dto, ct);
            return StatusCode((int)result.HttpStatusCode, result);
        }

        
        [HttpPut("{questionId:int}")]
        public async Task<IActionResult> Update(int examId,int questionId,[FromBody] UpdateQuestionDto dto,CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _questionServices.UpdateAsync(examId, questionId, dto, ct);
            return StatusCode((int)result.HttpStatusCode, result);
        }

        [HttpDelete("{questionId:int}")]
        public async Task<IActionResult> Delete( int examId, int questionId, CancellationToken ct)
        {
            var result = await _questionServices.DeleteAsync(examId, questionId, ct);
            return StatusCode((int)result.HttpStatusCode, result);
        }
    }
}
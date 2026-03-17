using E_Learning.Service.DTOs;

using E_Learning.Service.Services.QuizServices;
using Microsoft.AspNetCore.Mvc;

namespace E_Learning.API.Controllers
{
    [ApiController]
    [Route("api/quizzes")]
    public class QuizzesController : ControllerBase
    {
        private readonly IQuizService _quizService;

        public QuizzesController(IQuizService quizService)
        {
            _quizService = quizService;
        }

        // GET api/quizzes
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var response = await _quizService.GetAllAsync(ct);
            return StatusCode((int)response.HttpStatusCode, response);
        }

        // GET api/quizzes/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var response = await _quizService.GetByIdAsync(id, ct);
            return StatusCode((int)response.HttpStatusCode, response);
        }

        // GET api/quizzes/course/{courseId}
        [HttpGet("course/{courseId}")]
        public async Task<IActionResult> GetByCourse(int courseId, CancellationToken ct)
        {
            var response = await _quizService.GetByCourseIdAsync(courseId, ct);
            return StatusCode((int)response.HttpStatusCode, response);
        }

        // POST api/quizzes
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateQuizDto dto, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _quizService.CreateAsync(dto, ct);
            return StatusCode((int)response.HttpStatusCode, response);
        }

        // PUT api/quizzes/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateQuizDto dto, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _quizService.UpdateAsync(id, dto, ct);
            return StatusCode((int)response.HttpStatusCode, response);
        }

        // DELETE api/quizzes/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var response = await _quizService.DeleteAsync(id, ct);
            return StatusCode((int)response.HttpStatusCode, response);
        }
    }
}
using E_Learning.Core.Entities.Assessments.Exams;
using E_Learning.Core.Entities.Reviews;
using E_Learning.Service.Services.ExamServices.Exam;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/exams")]
//[Authorize]
public class ExamController : ControllerBase
{
    private readonly IExamServices _examServices;

    public ExamController(IExamServices examServices)
    {
        _examServices = examServices;
    }
    [HttpGet("{id:int}")]
    
    public async Task<IActionResult> GetById(int id)
    {
        var exam = await _examServices.GetByIdAsync(id);

        return exam is null
            ? NotFound(new { Message = $"Exam with ID {id} was not found." })
            : Ok(exam);
    }

    [HttpPost]
    // [Authorize(Roles = "Instructor,Admin")]
    public async Task<IActionResult> Create(
        [FromBody] CreateExamDto dto,
        CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _examServices.CreateAsync(dto, ct);
        return StatusCode((int)result.HttpStatusCode, result);
 
    }



  
}
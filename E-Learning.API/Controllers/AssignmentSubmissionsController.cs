using E_Learning.Core.Base;
using E_Learning.Service.Contract.Assignments;
using E_Learning.Service.DTOs.AssignmentsDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Learning.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentSubmissionsController : ControllerBase
    {
        private readonly IAssignmentSubmissionService _service;
        private readonly ResponseHandler _responseHandler;

        public AssignmentSubmissionsController(
            IAssignmentSubmissionService service,ResponseHandler responseHandler)
        {
            _service = service;
            this._responseHandler = responseHandler;
        }

        [HttpPost]
        public async Task<IActionResult> Submit([FromForm] CreateSubmissionDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(_responseHandler.HandleModelStateErrors<object>(ModelState));
            }
            var id = await _service.CreateSubmitAsync(dto);
            return Ok(id);
        }
        [HttpPut("grade/{id}")]
        public async Task<IActionResult> Grade(int id, GradeSubmissionDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(_responseHandler.HandleModelStateErrors<object>(ModelState));
            }
           var sub= await _service.CreateGradeAsync(id, dto);
            return Ok(sub);
        }
        [HttpGet("assignment/{id}")]
        public async Task<IActionResult> GetAll(int id)
        {
            var data = await _service.GetAllByAssignmentAsync(id);
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var assignment = await _service.GetByStudentAsync(id);
            return Ok(assignment);
        }


    }
}

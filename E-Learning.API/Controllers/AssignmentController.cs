using E_Learning.Core.Base;
using E_Learning.Service.Contract.Assignments;
using E_Learning.Service.DTOs.AssignmentsDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Learning.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentController : ControllerBase
    {
        private readonly IAssignmentService _service;
        private readonly ResponseHandler ValidationHelper;

        public AssignmentController(IAssignmentService service,ResponseHandler response)
        {
            _service = service;
            this.ValidationHelper = response;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAssignmentDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ValidationHelper.HandleModelStateErrors<object>(ModelState));
            }
            var AssignmentDto = await _service.CreateAsync(dto);
            return Ok(AssignmentDto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int pageNumber=1, int pageSize=10  )
        {
            var data = await _service.GetAllAsync(pageNumber,pageSize);
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var assignment = await _service.GetByIdAsync(id);
            return Ok(assignment);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateAssignmentDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ValidationHelper.HandleModelStateErrors<object>(ModelState));
            }
            var assignment = await _service.UpdateAsync(id, dto);
            return Ok(assignment);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var assignment = await _service.DeleteAsync(id);
            return Ok(assignment);
        }
    }
}

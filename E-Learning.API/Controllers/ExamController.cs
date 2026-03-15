using E_Learning.Core.Base;
using E_Learning.Service.Contract.Exam;
using E_Learning.Service.DTOs.ExamsDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Learning.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamController : ControllerBase
    {
        private readonly IExamService _service;
        private readonly ResponseHandler _validationHelper;

        public ExamController(IExamService service, ResponseHandler response)
        {
            _service = service;
            _validationHelper = response;
        }

        
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateExamDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(_validationHelper.HandleModelStateErrors<object>(ModelState));
            }

            var examDto = await _service.CreateAsync(dto);
            return Ok(examDto);
        }
 
        [HttpGet]
        public async Task<IActionResult> GetAll(int pageNumber = 1, int pageSize = 10)
        {
            var data = await _service.GetAllAsync(pageNumber, pageSize);
           
            return Ok(data);
        }

       
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var exam = await _service.GetByIdAsync(id);
            return Ok(exam);
        }

       
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateExamDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(_validationHelper.HandleModelStateErrors<object>(ModelState));
            }

            var exam = await _service.UpdateAsync(id, dto);
            return Ok(exam);
        }

       
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            return Ok(result);
        }
    
    }
}

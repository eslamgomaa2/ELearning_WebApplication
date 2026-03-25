using E_Learning.Core.Entities.Reviews;
using E_Learning.Core.Interfaces.Services.Reviews_Certificates;
using E_Learning.Service.DTOs.Reviews_Certificates.Certificates;
using E_Learning.Service.Services.Reviews_Certificates;
using Microsoft.AspNetCore.Mvc;

namespace E_Learning.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CertificatesController : ControllerBase
    {
        private readonly ICertificateServices _certificateServices;

        public CertificatesController(ICertificateServices certificateServices)
        {
            _certificateServices = certificateServices;
        }

        // ── GET ─────────────────────────────────────────────────────────────────

        [HttpGet("{id:int}")]
        
        public async Task<IActionResult> GetById(int id)
        {
            var certificate = await _certificateServices.GetByIdAsync(id);

            return certificate is null
                ? NotFound(new { Message = $"Certificate with ID {id} was not found." })
                : Ok(certificate);
        }

        [HttpGet("course/{courseId:int}")]
        public async Task<IActionResult> GetByCourseId(int courseId, [FromQuery] PaginationParams paginationParams)
        {
            var result = await _certificateServices.GetByCourseIdAsync(courseId,paginationParams);
            return StatusCode((int)result.HttpStatusCode, result);
        }

        [HttpGet("student/{studentId:guid}")]
        [ProducesResponseType(typeof(IEnumerable<Certificate>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByStudentId(Guid studentId)
        {
            var result = await _certificateServices.GetByStudentIdAsync(studentId);
            return StatusCode((int)result.HttpStatusCode, result);
        }

        // ── POST ────────────────────────────────────────────────────────────────

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCertificateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _certificateServices.CreateAsync(dto);

            return StatusCode((int)result.HttpStatusCode, result);
        }

        // ── PUT ─────────────────────────────────────────────────────────────────

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCertificateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _certificateServices.UpdateAsync(id, dto);

            return result is null
                ? NotFound(new { Message = $"Certificate with ID {id} was not found." })
                :  StatusCode((int)result.HttpStatusCode, result); ;
        }

        // ── DELETE ──────────────────────────────────────────────────────────────

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var certificate = await _certificateServices.DeleteAsync(id);

            return certificate is null
                ? NotFound(new { Message = $"Certificate with ID {id} was not found." })
                : Ok(certificate);
        }
    }
}

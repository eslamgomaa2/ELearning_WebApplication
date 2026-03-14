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
        [ProducesResponseType(typeof(Certificate), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var certificate = await _certificateServices.GetByIdAsync(id);

            return certificate is null
                ? NotFound(new { Message = $"Certificate with ID {id} was not found." })
                : Ok(certificate);
        }

        [HttpGet("course/{courseId:int}")]
        [ProducesResponseType(typeof(IEnumerable<Certificate>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByCourseId(int courseId)
        {
            var certificates = await _certificateServices.GetByCourseIdAsync(courseId);
            return Ok(certificates);
        }

        [HttpGet("student/{studentId:guid}")]
        [ProducesResponseType(typeof(IEnumerable<Certificate>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByStudentId(Guid studentId)
        {
            var certificates = await _certificateServices.GetByStudentIdAsync(studentId);
            return Ok(certificates);
        }

        // ── POST ────────────────────────────────────────────────────────────────

        [HttpPost]
        [ProducesResponseType(typeof(Certificate), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateCertificateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var certificate = await _certificateServices.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { data = certificate.Data },
                certificate
            );
        }

        // ── PUT ─────────────────────────────────────────────────────────────────

        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(Certificate), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCertificateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var certificate = await _certificateServices.UpdateAsync(id, dto);

            return certificate is null
                ? NotFound(new { Message = $"Certificate with ID {id} was not found." })
                : Ok(certificate);
        }

        // ── DELETE ──────────────────────────────────────────────────────────────

        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(Certificate), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var certificate = await _certificateServices.DeleteAsync(id);

            return certificate is null
                ? NotFound(new { Message = $"Certificate with ID {id} was not found." })
                : Ok(certificate);
        }
    }
}

using E_Learning.Service.DTOs.LiveSessionDto;
using E_Learning.Service.Services.LiveSessionServices;
using Microsoft.AspNetCore.Mvc;

namespace E_Learning.API.Controllers
{
    [ApiController]
    [Route("api/live-sessions")]
    public class LiveSessionsController : ControllerBase
    {
        private readonly ILiveSessionService _liveSessionService;

        public LiveSessionsController(ILiveSessionService liveSessionService)
        {
            _liveSessionService = liveSessionService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var response = await _liveSessionService.GetAllAsync(null, null, ct);
            return Ok(response);
        }

        [HttpGet("searchByTitle")]
        public async Task<IActionResult> Search([FromQuery] string query, CancellationToken ct)
        {
            var response = await _liveSessionService.GetAllAsync(query, null, ct);
            return Ok(response);
        }

        [HttpGet("filter")]
        public async Task<IActionResult> Filter([FromQuery] int status, CancellationToken ct)
        {
            var response = await _liveSessionService.GetAllAsync(null, status, ct);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var response = await _liveSessionService.GetByIdAsync(id, ct);
            return Ok(response);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateLiveSessionDto dto, CancellationToken ct)
        {
            var response = await _liveSessionService.CreateAsync(dto, ct);
            return Ok(response);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateLiveSessionDto dto, CancellationToken ct)
        {
            var response = await _liveSessionService.UpdateAsync(id, dto, ct);
            return Ok(response);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var response = await _liveSessionService.DeleteAsync(id, ct);
            return Ok(response);
        }
    }
}
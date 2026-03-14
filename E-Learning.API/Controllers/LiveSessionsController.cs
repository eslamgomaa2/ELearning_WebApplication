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
        return StatusCode((int)response.HttpStatusCode, response);
    }

    [HttpGet("searchByTitle")]
    public async Task<IActionResult> Search([FromQuery] string query, CancellationToken ct)
    {
        var response = await _liveSessionService.GetAllAsync(query, null, ct);
        return StatusCode((int)response.HttpStatusCode, response);
    }

    [HttpGet("filter")]
    public async Task<IActionResult> Filter([FromQuery] string status, CancellationToken ct)
    {
        var response = await _liveSessionService.GetAllAsync(null, status, ct);
        return StatusCode((int)response.HttpStatusCode, response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var response = await _liveSessionService.GetByIdAsync(id, ct);
        return StatusCode((int)response.HttpStatusCode, response);
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CreateLiveSessionDto dto, CancellationToken ct)
    {
        var response = await _liveSessionService.CreateAsync(dto, ct);
        return StatusCode((int)response.HttpStatusCode, response);
    }

    [HttpPut("update/{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateLiveSessionDto dto, CancellationToken ct)
    {
        dto.Id = id; 
        var response = await _liveSessionService.UpdateAsync(id, dto, ct);
        return StatusCode((int)response.HttpStatusCode, response);
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var response = await _liveSessionService.DeleteAsync(id, ct);
        return StatusCode((int)response.HttpStatusCode, response);
    }
}
}
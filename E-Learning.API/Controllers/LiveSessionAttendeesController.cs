using E_Learning.Service.DTOs.LiveSessionDto;
using E_Learning.Service.Services.LiveSessionServices;
using Microsoft.AspNetCore.Mvc;

namespace E_Learning.API.Controllers
{
    [ApiController]
    [Route("api/live-session-attendees")]
    public class LiveSessionAttendeesController : ControllerBase
    {
        private readonly ILiveSessionAttendeeService _attendeeService;

        public LiveSessionAttendeesController(ILiveSessionAttendeeService attendeeService)
        {
            _attendeeService = attendeeService;
        }

        [HttpPost("JoineSession")]
        public async Task<IActionResult> JoineSession([FromBody] LogAttendanceDto dto, CancellationToken ct)
        {
            var response = await _attendeeService.LogAttendanceAsync(dto, ct);
            return Ok(response);
        }

        [HttpGet("Attendees/{sessionId}")]
        public async Task<IActionResult> GetAttendeesBySession(int sessionId, CancellationToken ct)
        {
            var response = await _attendeeService.GetAttendeesBySessionIdAsync(sessionId, ct);
            return Ok(response);
        }
         [HttpPost("LeaveSession")]
        public async Task<IActionResult> LeaveSession([FromBody] LeaveSessionDto dto, CancellationToken ct = default)
        {
            var response = await _attendeeService.LeaveSession(dto, ct);
            return Ok(response);
        }
    }
}
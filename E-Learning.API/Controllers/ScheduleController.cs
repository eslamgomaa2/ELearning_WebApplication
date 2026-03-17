using E_Learning.Core.Base;
using E_Learning.Service.Contract;
using E_Learning.Service.DTOs.Schedule;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Learning.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/schedule")]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleService _service;
        private readonly ResponseHandler _response;

        public ScheduleController(IScheduleService service, ResponseHandler response)
        {
            _service = service;
            _response = response;
        }

        private Guid GetUserId()
        {
            var raw = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(raw) || !Guid.TryParse(raw, out var userId))
                throw new UnauthorizedAccessException("Unauthorized.");
            return userId;
        }

        // GET /api/schedule/calendar?month=3&year=2026
        [HttpGet("calendar")]
        public async Task<IActionResult> GetCalendar([FromQuery] int month, [FromQuery] int year)
        {
            try
            {
                var userId = GetUserId();
                var result = await _service.GetCalendarAsync(userId, month, year);
                var res = _response.Success(result);
                return StatusCode((int)res.HttpStatusCode, res);
            }
            catch (Exception ex)
            {
                var res = _response.BadRequest<object>(ex.Message);
                return StatusCode((int)res.HttpStatusCode, res);
            }
        }

        // GET /api/schedule/upcoming?page=1&pageSize=20&type=Exam&courseId=5
        [HttpGet("upcoming")]
        public async Task<IActionResult> GetUpcomingEvents([FromQuery] ScheduleQueryDto query)
        {
            try
            {
                var userId = GetUserId();
                var result = await _service.GetUpcomingEventsAsync(userId, query);
                var res = _response.Success(result);
                return StatusCode((int)res.HttpStatusCode, res);
            }
            catch (Exception ex)
            {
                var res = _response.BadRequest<object>(ex.Message);
                return StatusCode((int)res.HttpStatusCode, res);
            }
        }

        // GET /api/schedule/deadlines-summary
        [HttpGet("deadlines-summary")]
        public async Task<IActionResult> GetDeadlineSummary()
        {
            try
            {
                var userId = GetUserId();
                var result = await _service.GetDeadlineSummaryAsync(userId);
                var res = _response.Success(result);
                return StatusCode((int)res.HttpStatusCode, res);
            }
            catch (Exception ex)
            {
                var res = _response.BadRequest<object>(ex.Message);
                return StatusCode((int)res.HttpStatusCode, res);
            }
        }

        // GET /api/schedule/event/{type}/{id}
        [HttpGet("event/{type}/{id:int}")]
        public async Task<IActionResult> GetEventDetails([FromRoute] string type, [FromRoute] int id)
        {
            try
            {
                var userId = GetUserId();
                var result = await _service.GetEventDetailsAsync(userId, type, id);
                var res = _response.Success(result);
                return StatusCode((int)res.HttpStatusCode, res);
            }
            catch (Exception ex)
            {
                var msg = ex.Message ?? "BadRequest";
                var res = msg.Contains("not found", StringComparison.OrdinalIgnoreCase)
                    ? _response.NotFound<object>(msg)
                    : _response.BadRequest<object>(msg);
                return StatusCode((int)res.HttpStatusCode, res);
            }
        }
    }
}
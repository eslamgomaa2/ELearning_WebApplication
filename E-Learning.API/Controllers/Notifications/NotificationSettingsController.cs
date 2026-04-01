using E_Learning.Core.Base;
using E_Learning.Service.Contract.Notifications;
using E_Learning.Service.DTOs.Notification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Learning.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/notification-settings")]
    public class NotificationSettingsController : ControllerBase
    {
        private readonly INotificationSettingService _service;
        private readonly ResponseHandler _response;

        public NotificationSettingsController(INotificationSettingService service, ResponseHandler response)
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

        // GET /api/notification-settings
        [HttpGet]
        public async Task<IActionResult> GetMySettings(CancellationToken ct)
        {
            try
            {
                var userId = GetUserId();
                var result = await _service.GetMySettingsAsync(userId,ct);
                var res = _response.Success(result);
                return StatusCode((int)res.HttpStatusCode, res);
            }
            catch (Exception ex)
            {
                var res = _response.BadRequest<object>(ex.Message);
                return StatusCode((int)res.HttpStatusCode, res);
            }
        }

        // PUT /api/notification-settings
        [HttpPut]
        public async Task<IActionResult> UpdateMySettings([FromBody] UpdateNotificationSettingDto dto)
        {
            if (!ModelState.IsValid)
            {
                var res = _response.HandleModelStateErrors<object>(ModelState);
                return StatusCode((int)res.HttpStatusCode, res);
            }

            try
            {
                var userId = GetUserId();
                var result = await _service.UpdateMySettingsAsync(userId, dto);
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
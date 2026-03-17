using E_Learning.Core.Base;
using E_Learning.Core.Enums;
using E_Learning.Service.Contract.Notifications;
using E_Learning.Service.DTOs.Notification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Learning.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/notifications")]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _service;
        private readonly ResponseHandler _response;

        public NotificationsController(INotificationService service, ResponseHandler response)
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

        // GET /api/notifications?page=1&pageSize=20&isRead=false&type=General
        [HttpGet]
        public async Task<IActionResult> GetMyNotifications([FromQuery] NotificationQueryDto q)
        {
            try
            {
                var userId = GetUserId();
                var result = await _service.GetMyNotificationsAsync(userId, q);
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

        // GET /api/notifications/unread-count
        [HttpGet("unread-count")]
        public async Task<IActionResult> GetUnreadCount()
        {
            try
            {
                var userId = GetUserId();
                var count = await _service.GetMyUnreadCountAsync(userId);
                var res = _response.Success(new { unreadCount = count });
                return StatusCode((int)res.HttpStatusCode, res);
            }
            catch (Exception ex)
            {
                var res = _response.BadRequest<object>(ex.Message);
                return StatusCode((int)res.HttpStatusCode, res);
            }
        }

        // PUT /api/notifications/{id}/read
        [HttpPut("{id:int}/read")]
        public async Task<IActionResult> MarkAsRead([FromRoute] int id)
        {
            try
            {
                var userId = GetUserId();
                await _service.MarkAsReadAsync(userId, id);
                var res = _response.Success(new { message = "Marked as read." });
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

        // PUT /api/notifications/read-all
        [HttpPut("read-all")]
        public async Task<IActionResult> MarkAllAsRead()
        {
            try
            {
                var userId = GetUserId();
                await _service.MarkAllAsReadAsync(userId);
                var res = _response.Success(new { message = "All notifications marked as read." });
                return StatusCode((int)res.HttpStatusCode, res);
            }
            catch (Exception ex)
            {
                var res = _response.BadRequest<object>(ex.Message);
                return StatusCode((int)res.HttpStatusCode, res);
            }
        }

        // DELETE /api/notifications/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteNotification([FromRoute] int id)
        {
            try
            {
                var userId = GetUserId();
                await _service.DeleteNotificationAsync(userId, id);
                var res = _response.Success(new { message = "Notification deleted." });
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

        // ══════════════════════════════════════════════════════════
        // TEST ONLY — remove before production
        // POST /api/notifications/test-send
        // Sends a test notification to the currently logged-in user
        // ══════════════════════════════════════════════════════════
        [HttpPost("test-send")]
        public async Task<IActionResult> TestSendNotification()
        {
            try
            {
                var userId = GetUserId();
                await _service.SendNotificationAsync(
                    userId,
                    "Test Notification",
                    "This is a real-time test notification at " + DateTime.UtcNow.ToString("HH:mm:ss"),
                    NotificationType.General);

                var res = _response.Success(new { message = "Test notification sent!" });
                return StatusCode((int)res.HttpStatusCode, res);
            }
            catch (Exception ex)
            {
                var res = _response.BadRequest<object>(ex.Message);
                return StatusCode((int)res.HttpStatusCode, res);
            }
        }
    }
}
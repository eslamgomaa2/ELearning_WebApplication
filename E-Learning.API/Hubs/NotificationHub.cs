using E_Learning.Service.Contract.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace E_Learning.API.Hubs
{
    
    [Authorize]
    public class NotificationHub : Hub<INotificationClient>
    {
        private readonly ILogger<NotificationHub> _logger;

        public NotificationHub(ILogger<NotificationHub> logger)
        {
            _logger = logger;
        }

        
        public override async Task OnConnectedAsync()
        {
            var userId = GetUserId();

            _logger.LogInformation(
                "NotificationHub: User {UserId} connected. ConnectionId: {ConnectionId}",
                userId, Context.ConnectionId);

            await base.OnConnectedAsync();
        }

        
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = GetUserId();

            _logger.LogInformation(
                "NotificationHub: User {UserId} disconnected. ConnectionId: {ConnectionId}",
                userId, Context.ConnectionId);

            if (exception != null)
            {
                _logger.LogWarning(exception,
                    "NotificationHub: User {UserId} disconnected with error.", userId);
            }

            await base.OnDisconnectedAsync(exception);
        }

        
        private string GetUserId()
        {
            return Context.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? "unknown";
        }
    }
}
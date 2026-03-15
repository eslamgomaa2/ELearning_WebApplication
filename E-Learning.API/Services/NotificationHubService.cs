using E_Learning.API.Hubs;
using E_Learning.Service.Contract.Notifications;
using Microsoft.AspNetCore.SignalR;

namespace E_Learning.API.Services
{
    public class NotificationHubService : INotificationHubService
    {
        private readonly IHubContext<NotificationHub, INotificationClient> _hubContext;

        public NotificationHubService(
            IHubContext<NotificationHub, INotificationClient> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendNotificationToUser(string userId, NotificationPayload payload)
        {
            await _hubContext.Clients.User(userId).ReceiveNotification(payload);
        }

        public async Task SendUnreadCountToUser(string userId, int unreadCount)
        {
            await _hubContext.Clients.User(userId).UpdateUnreadCount(unreadCount);
        }

        public async Task SendNotificationReadToUser(string userId, int notificationId)
        {
            await _hubContext.Clients.User(userId).NotificationRead(notificationId);
        }

        public async Task SendAllReadToUser(string userId)
        {
            await _hubContext.Clients.User(userId).AllNotificationsRead();
        }

        public async Task SendNotificationDeletedToUser(string userId, int notificationId)
        {
            await _hubContext.Clients.User(userId).NotificationDeleted(notificationId);
        }
    }
}
using E_Learning.Core.Entities.Notifications;
using E_Learning.Core.Enums;
using E_Learning.Core.Repository;
using E_Learning.Service.Contract.Notifications;
using E_Learning.Service.DTOs;
using E_Learning.Service.DTOs.Notification;
using Microsoft.EntityFrameworkCore;

namespace E_Learning.Service.Services.Notifications
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _uow;
        private readonly INotificationHubService _hubService;

        public NotificationService(
            IUnitOfWork uow,
            INotificationHubService hubService)
        {
            _uow = uow;
            _hubService = hubService;
        }

        public async Task SendNotificationAsync(
            Guid userId,
            string title,
            string body,
            NotificationType type = NotificationType.General)
        {
            var settings = await _uow.NotificationSettings
                .Query()
                .FirstOrDefaultAsync(s => s.UserId == userId);

            if (settings == null)
            {
                settings = new NotificationSetting { UserId = userId };
                await _uow.NotificationSettings.AddAsync(settings);
                await _uow.SaveChangesAsync();
            }

            if (!IsTypeEnabled(settings, type))
                return;

            if (!settings.InAppNotification)
                return;

            var notification = new Notification
            {
                UserId = userId,
                Title = title,
                Body = body,
                Type = type,
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };

            await _uow.Notifications.AddAsync(notification);
            await _uow.SaveChangesAsync();

            var payload = new NotificationPayload
            {
                Id = notification.Id,
                Title = notification.Title,
                Body = notification.Body,
                Type = notification.Type.ToString(),
                IsRead = false,
                CreatedAt = notification.CreatedAt
            };

            var userIdString = userId.ToString();
            await _hubService.SendNotificationToUser(userIdString, payload);

            var unreadCount = await _uow.Notifications.GetUnreadCountAsync(userId);
            await _hubService.SendUnreadCountToUser(userIdString, unreadCount);
        }

        public async Task SendToMultipleUsersAsync(
            IEnumerable<Guid> userIds,
            string title,
            string body,
            NotificationType type = NotificationType.General)
        {
            foreach (var userId in userIds)
            {
                await SendNotificationAsync(userId, title, body, type);
            }
        }

        public async Task DeleteNotificationAsync(Guid userId, int notificationId)
        {
            var notification = await _uow.Notifications.Query()
                .FirstOrDefaultAsync(n => n.Id == notificationId && n.UserId == userId);

            if (notification == null)
                throw new Exception("Notification not found.");

            _uow.Notifications.Remove(notification);
            await _uow.SaveChangesAsync();

            var userIdString = userId.ToString();
            await _hubService.SendNotificationDeletedToUser(userIdString, notificationId);

            var unreadCount = await _uow.Notifications.GetUnreadCountAsync(userId);
            await _hubService.SendUnreadCountToUser(userIdString, unreadCount);
        }

        public async Task<PagedResultDto<NotificationDto>> GetMyNotificationsAsync(
            Guid userId, NotificationQueryDto q)
        {
            if (q.Page < 1 || q.PageSize < 1 || q.PageSize > 50)
                throw new Exception("Invalid paging parameters.");

            NotificationType? type = null;
            if (!string.IsNullOrWhiteSpace(q.Type))
            {
                if (!Enum.TryParse<NotificationType>(q.Type, true, out var parsed))
                    throw new Exception("Invalid notification type.");
                type = parsed;
            }

            var (items, total) = await _uow.Notifications
                .GetUserNotificationsPagedAsync(userId, q.Page, q.PageSize, q.IsRead, type);

            var dto = items.Select(n => new NotificationDto
            {
                Id = n.Id,
                Title = n.Title,
                Body = n.Body,
                Type = n.Type.ToString(),
                IsRead = n.IsRead,
                CreatedAt = n.CreatedAt
            }).ToList();

            return new PagedResultDto<NotificationDto>
            {
                Items = dto,
                TotalCount = total,
                PageNumber = q.Page,
                PageSize = q.PageSize
            };
        }

        public Task<int> GetMyUnreadCountAsync(Guid userId)
            => _uow.Notifications.GetUnreadCountAsync(userId);

        public async Task MarkAsReadAsync(Guid userId, int notificationId)
        {
            var tracked = await _uow.Notifications.Query()
                .FirstOrDefaultAsync(n => n.Id == notificationId && n.UserId == userId);

            if (tracked == null)
                throw new Exception("Notification not found.");

            if (!tracked.IsRead)
                tracked.IsRead = true;

            await _uow.SaveChangesAsync();

            // Real-time: notify all user tabs
            var userIdString = userId.ToString();
            await _hubService.SendNotificationReadToUser(userIdString, notificationId);

            var unreadCount = await _uow.Notifications.GetUnreadCountAsync(userId);
            await _hubService.SendUnreadCountToUser(userIdString, unreadCount);
        }

        public async Task MarkAllAsReadAsync(Guid userId)
        {
            await _uow.Notifications.MarkAllAsReadAsync(userId);
            await _uow.SaveChangesAsync();

            // Real-time: notify all user tabs
            var userIdString = userId.ToString();
            await _hubService.SendAllReadToUser(userIdString);
            await _hubService.SendUnreadCountToUser(userIdString, 0);
        }

        private static bool IsTypeEnabled(NotificationSetting settings, NotificationType type)
        {
            return type switch
            {
                NotificationType.CourseAnnouncement => settings.CourseAnnouncement,
                NotificationType.AssignmentReminder => settings.AssignmentReminder,
                NotificationType.ExamNotification => settings.ExamNotification,
                NotificationType.PlatformUpdate => settings.PlatformUpdates,
                NotificationType.General => true,
                _ => true
            };
        }
    }
}
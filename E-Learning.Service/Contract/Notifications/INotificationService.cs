using E_Learning.Core.Enums;
using E_Learning.Service.DTOs;
using E_Learning.Service.DTOs.Notification;

namespace E_Learning.Service.Contract.Notifications
{
    public interface INotificationService
    {

        Task<PagedResultDto<NotificationDto>> GetMyNotificationsAsync(
            Guid userId, NotificationQueryDto q);

        Task<int> GetMyUnreadCountAsync(Guid userId);

        Task MarkAsReadAsync(Guid userId, int notificationId);

        Task MarkAllAsReadAsync(Guid userId);


        Task SendNotificationAsync(
            Guid userId,
            string title,
            string body,
            NotificationType type = NotificationType.General);


        Task SendToMultipleUsersAsync(
            IEnumerable<Guid> userIds,
            string title,
            string body,
            NotificationType type = NotificationType.General);


        Task DeleteNotificationAsync(Guid userId, int notificationId);
    }
}
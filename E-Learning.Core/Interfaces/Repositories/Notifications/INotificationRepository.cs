using E_Learning.Core.Entities.Notifications;
using E_Learning.Core.Enums;

namespace E_Learning.Core.Interfaces.Repositories.Notifications
{
    public interface INotificationRepository : IGenericRepository<Notification, Guid> 
    {
        Task<(IReadOnlyList<Notification> Items, int TotalCount)> GetUserNotificationsPagedAsync(
            Guid userId, int page, int pageSize, bool? isRead, NotificationType? type);

        Task<int> GetUnreadCountAsync(Guid userId);

        Task<Notification?> GetByIdForUserAsync(int notificationId, Guid userId);

        Task<int> MarkAllAsReadAsync(Guid userId);
    }
}
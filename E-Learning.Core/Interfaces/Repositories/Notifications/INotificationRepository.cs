using E_Learning.Core.Entities.Notifications;
using E_Learning.Core.Enums;

namespace E_Learning.Core.Interfaces.Repositories.Notifications
{
    public interface INotificationRepository : IGenericRepository<Notification, int>
    {
        Task<(IReadOnlyList<Notification> Items, int TotalCount)> GetUserNotificationsPagedAsync(
            Guid userId, int page, int pageSize, bool? isRead, NotificationType? type);

        Task<int> GetUnreadCountAsync(Guid userId);

        Task<Notification?> GetByIdForUserAsync(int notificationId, Guid userId);

        Task<int> MarkAllAsReadAsync(Guid userId);
        Task<IReadOnlyList<Notification>> GetNotificationByguid(Guid userId);
    }
}
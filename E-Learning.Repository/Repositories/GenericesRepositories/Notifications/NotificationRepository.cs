using E_Learning.Core.Entities.Notifications;
using E_Learning.Core.Enums;
using E_Learning.Core.Interfaces.Repositories.Notifications;
using E_Learning.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace E_Learning.Repository.Repositories.GenericesRepositories.Notifications
{
    public class NotificationRepository
    : GenericRepository<Notification, int>, INotificationRepository
    {
        public NotificationRepository(ELearningDbContext context) : base(context) { }

        public async Task<(IReadOnlyList<Notification> Items, int TotalCount)> GetUserNotificationsPagedAsync(
            Guid userId, int page, int pageSize, bool? isRead, NotificationType? type)
        {
            var q = QueryNoTracking().Where(n => n.UserId == userId);

            if (isRead.HasValue) q = q.Where(n => n.IsRead == isRead.Value);
            if (type.HasValue) q = q.Where(n => n.Type == type.Value);

            var total = await q.CountAsync();

            var items = await q.OrderByDescending(n => n.CreatedAt)
                               .Skip((page - 1) * pageSize)
                               .Take(pageSize)
                               .ToListAsync();

            return (items, total);
        }

        public Task<int> GetUnreadCountAsync(Guid userId)
            => QueryNoTracking().CountAsync(n => n.UserId == userId && !n.IsRead);

        public Task<Notification?> GetByIdForUserAsync(int notificationId, Guid userId)
            => QueryNoTracking().FirstOrDefaultAsync(n => n.Id == notificationId && n.UserId == userId);

        public async Task<int> MarkAllAsReadAsync(Guid userId)
        {
            var items = await Query().Where(n => n.UserId == userId && !n.IsRead).ToListAsync();
            foreach (var n in items) n.IsRead = true;
            return items.Count;
        }

        public Task GetByIdAsync(int notificationId)
        {
            throw new NotImplementedException();
        }
    }
}

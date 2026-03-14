using E_Learning.Core.Enums;
using E_Learning.Core.Repository;
using E_Learning.Service.Contract.Notifications;
using E_Learning.Service.DTOs;
using E_Learning.Service.DTOs.Notification;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Service.Services.Notifications
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _uow;
        public NotificationService(IUnitOfWork uow) => _uow = uow;

        public async Task<PagedResultDto<NotificationDto>> GetMyNotificationsAsync(Guid userId, NotificationQueryDto q)
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

            var (items, total) = await _uow.Notifications.GetUserNotificationsPagedAsync(
                userId, q.Page, q.PageSize, q.IsRead, type);

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
        }

        public async Task MarkAllAsReadAsync(Guid userId)
        {
            await _uow.Notifications.MarkAllAsReadAsync(userId);
            await _uow.SaveChangesAsync();
        }
    }
}

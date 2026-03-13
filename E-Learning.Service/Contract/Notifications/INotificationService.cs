using E_Learning.Service.DTOs;
using E_Learning.Service.DTOs.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Service.Contract.Notifications
{
    public interface INotificationService
    {
        Task<PagedResultDto<NotificationDto>> GetMyNotificationsAsync(Guid userId, NotificationQueryDto q);
        Task<int> GetMyUnreadCountAsync(Guid userId);
        Task MarkAllAsReadAsync(Guid userId);
        Task MarkAsReadAsync(Guid userId, int notificationId);
    }
}

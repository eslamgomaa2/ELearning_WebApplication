using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Service.Contract.Notifications
{
    public interface INotificationClient
    {
        
        Task ReceiveNotification(NotificationPayload notification);

        
        Task UpdateUnreadCount(int unreadCount);

        
        Task NotificationRead(int notificationId);

        
        Task AllNotificationsRead();

        
        Task NotificationDeleted(int notificationId);
    }

    public class NotificationPayload
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; }
    }
}

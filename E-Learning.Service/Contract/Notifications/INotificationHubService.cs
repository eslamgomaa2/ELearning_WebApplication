using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Service.Contract.Notifications
{
    public interface INotificationHubService
    {

        Task SendNotificationToUser(string userId, NotificationPayload payload);


        Task SendUnreadCountToUser(string userId, int unreadCount);


        Task SendNotificationReadToUser(string userId, int notificationId);


        Task SendAllReadToUser(string userId);

        Task SendNotificationDeletedToUser(string userId, int notificationId);
    }
}

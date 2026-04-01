using E_Learning.Service.DTOs.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Service.Contract.Notifications
{
    public interface INotificationSettingService
    {
        Task<NotificationSettingDto> GetMySettingsAsync(Guid userId,CancellationToken ct);
       Task<NotificationSettingDto> UpdateMySettingsAsync(Guid userId, UpdateNotificationSettingDto dto);
    }
}

using E_Learning.Core.Entities.Notifications;
using E_Learning.Core.Repository;
using E_Learning.Service.Contract.Notifications;
using E_Learning.Service.DTOs.Notification;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Service.Services.Notifications
{
    public class NotificationSettingService : INotificationSettingService
    {
        private readonly IUnitOfWork _uow;
        public NotificationSettingService(IUnitOfWork uow) => _uow = uow;

        public async Task<NotificationSettingDto> GetMySettingsAsync(Guid userId,CancellationToken ct)
        {
            var s = await _uow.NotificationSettings.GetByUserIdAsync(userId,ct);
            if (s == null)
            {
                s = new NotificationSetting { UserId = userId };
                await _uow.NotificationSettings.AddAsync(s);
                await _uow.SaveChangesAsync();
            }

            return Map(s);
        }

        public async Task<NotificationSettingDto> UpdateMySettingsAsync(Guid userId, UpdateNotificationSettingDto dto)
        {
            var s = await _uow.NotificationSettings.Query()
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (s == null) throw new Exception("Notification settings not found.");

            s.CourseAnnouncement = dto.CourseAnnouncement;
            s.AssignmentReminder = dto.AssignmentReminder;
            s.ExamNotification = dto.ExamNotification;
            s.PlatformUpdates = dto.PlatformUpdates;
            s.InAppNotification = dto.InAppNotification;
            s.EmailNotification = dto.EmailNotification;

            await _uow.SaveChangesAsync();
            return Map(s);
        }

        private static NotificationSettingDto Map(NotificationSetting s) => new()
        {
            CourseAnnouncement = s.CourseAnnouncement,
            AssignmentReminder = s.AssignmentReminder,
            ExamNotification = s.ExamNotification,
            PlatformUpdates = s.PlatformUpdates,
            InAppNotification = s.InAppNotification,
            EmailNotification = s.EmailNotification
        };
    }
}

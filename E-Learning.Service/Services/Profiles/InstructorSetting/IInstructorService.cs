using E_Learning.Core.Base;
using E_Learning.Core.Entities.Notifications;
using E_Learning.Core.Entities.Profiles;
using E_Learning.Service.DTOs.Notification;
using E_Learning.Service.DTOs.Profiles;
using E_Learning.Service.DTOs.Profiles.Instructor;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace E_Learning.Service.Services.Profiles.InstructorSetting
{
    public interface IInstructorService
    {
        Task<Response<NotificationSetting>> GetAllSettingsAsync(Guid userId, CancellationToken ct);
        Task<Response<InstructorProfile>> UpdatePersonalDetailsAsync(Guid userId, UpdateInstructorProfileDto dto, CancellationToken ct);
        Task<Response<NotificationSetting>> UpdateInstructorNotificationsAsync(Guid userId, InstructorNotificationSettingsDto dto, CancellationToken ct);
       

    }
}
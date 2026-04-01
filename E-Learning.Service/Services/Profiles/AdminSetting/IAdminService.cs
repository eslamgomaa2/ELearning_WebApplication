using E_learning.Core.Entities.Identity;
using E_Learning.Core.Base;
using E_Learning.Core.Entities.Academic;
using E_Learning.Core.Entities.Notifications;
using E_Learning.Core.Entities.Profiles;
using E_Learning.Service.DTOs.Profiles.Admin;
using E_Learning.Service.DTOs.Profiles.Instructor;
using E_Learning.Service.DTOs.Profiles.Student;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace E_Learning.Service.Services.Profiles.AdminSetting
{
    public interface IAdminService
    {
        Task<Response<NotificationSetting>> GetAdminNotificationSettingsAsync(Guid userId, CancellationToken ct = default);
        Task<Response<AdminProfile>> UpdateAdminProfile(Guid userId, UpdateAdminProfileDto dto, CancellationToken ct = default);
        Task<Response<NotificationSetting>> UpdateAdminNotificationSettingAsync(Guid userId, AdminNotificationSettingDto dto, CancellationToken ct = default);
        Task<Response<NotificationSetting>> UpdateAdminNotification_PrefrancesSettingAsync(Guid userId, AdminNotificationPrefrancesDto dto, CancellationToken ct = default);
        Task<Response<ApplicationUser>> UpdateGeneralSettingAsync(Guid userId, GeneralSettingDto dto, CancellationToken ct = default);
        Task<Response<AcademicSetting>> UpdateAcademicSettingAsync(Guid userId, AcademicSettingDto dto, CancellationToken ct = default);
        Task<Response<AdminProfile>> GetAdminProfileByUserId(Guid userId, CancellationToken ct = default);
    }
}
using E_Learning.Core.Base;
using E_Learning.Core.Entities.Notifications;
using E_Learning.Core.Entities.Profiles;
using E_Learning.Service.DTOs.Profiles.Student;

namespace E_Learning.Service.Services.Profiles.StudentSetting
{
    public interface IStudentService
    {
        Task<Response<StudentProfile>> GetAllSettingsAsync(Guid userId);

        Task<Response<StudentProfile>> UpdateStudentInformationAsync(Guid userId, UpdateStudentProfileDto dto, CancellationToken ct);

       

        Task<Response<NotificationSetting>> UpdateNotificationSettingAsync(Guid userId, StudentNotificationSettingDto dto,CancellationToken ct);
        Task<Response<PrivacySettingsDto>> UpdatePrivacySettingsAsync(Guid userId, PrivacySettingsDto dto);
        Task<Response<EmailSecurityDto>> GetEmailSecurityAsync(Guid userId);

        Task<Response<LearningPrefrancesDto>> UpdateLearningPrefrancesAsync(Guid userId, LearningPrefrancesDto dto);

        Task<Response<UserDataExportDto>> RequestDataExportAsync(Guid userId);
        public Task<Response<IReadOnlyList<StudentProfile>>> GetAllStudentsAsync();
        Task<Response<StudentProfile>> GetStudentProfileByUserIdAsync(Guid id);
        


    }
}


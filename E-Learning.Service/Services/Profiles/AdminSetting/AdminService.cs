using E_learning.Core.Entities.Identity;
using E_Learning.Core.Base;
using E_Learning.Core.Entities.Academic;
using E_Learning.Core.Entities.Notifications;
using E_Learning.Core.Entities.Profiles;
using E_Learning.Core.Repository;
using E_Learning.Service.DTOs.Profiles.Admin;
using E_Learning.Service.DTOs.Profiles.Student;
using E_Learning.Service.Services.Profiles.AdminSetting;
using E_Learning.Service.Services.Profiles.FileStorageService;
using Microsoft.AspNetCore.Identity;

public class AdminService : IAdminService
{
    private readonly IUnitOfWork _uow;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IFileStorage _fileStorage;
    private readonly ResponseHandler _responseHandler;

    public AdminService(
        IUnitOfWork uow,
        UserManager<ApplicationUser> userManager,
        IFileStorage fileStorage,
        ResponseHandler responseHandler)
    {
        _uow = uow;
        _userManager = userManager;
        _fileStorage = fileStorage;
        _responseHandler = responseHandler;
    }

    public async Task<Response<NotificationSetting>> GetAdminNotificationSettingsAsync(Guid userId, CancellationToken ct = default)
    {
        var settings = await _uow.NotificationSettings.GetByUserIdAsync(userId, ct);
        if (settings is null)
            return _responseHandler.NotFound<NotificationSetting>("Notification settings not found");
        return _responseHandler.Success(settings);
    }

    public async Task<Response<AdminProfile>> GetAdminProfileByUserId(Guid userId, CancellationToken ct = default)
    {
        var profile = await _uow.AdminProfiles.GetAdminProfileWithUserByUserIdAsync(userId, ct);
        if (profile is null)
            return _responseHandler.NotFound<AdminProfile>("Admin profile not found");

        var pictureUrl = string.IsNullOrEmpty(profile.ProfilePicture)
            ? null
            : _fileStorage.GetPublicUrl(profile.ProfilePicture);

        return _responseHandler.Success(profile);
    }

    public async Task<Response<AdminProfile>> UpdateAdminProfile(Guid userId, UpdateAdminProfileDto dto, CancellationToken ct = default)
    {
        var profile = await _uow.AdminProfiles.GetAdminProfileWithUserByUserIdAsync(userId, ct);
        if (profile is null)
            return _responseHandler.NotFound<AdminProfile>("Admin profile not found");

        // ── AppUser fields
        if (!string.IsNullOrWhiteSpace(dto.FullName)) profile.AppUser.FullName = dto.FullName;
        if (!string.IsNullOrWhiteSpace(dto.Email) && !string.Equals(profile.AppUser.Email, dto.Email, StringComparison.OrdinalIgnoreCase))
        {
            var emailTaken = await _userManager.FindByEmailAsync(dto.Email);
            if (emailTaken is not null && emailTaken.Id != userId)
                return _responseHandler.BadRequest<AdminProfile>($"Email '{dto.Email}' is already in use.");

            profile.AppUser.Email = dto.Email;
            profile.AppUser.UserName = dto.Email;
        }
        if (!string.IsNullOrWhiteSpace(dto.PhoneNumber)) profile.AppUser.PhoneNumber = dto.PhoneNumber;
        profile.AppUser.UpdatedAt = DateTime.UtcNow;

        // ── AdminProfile fields
        if (!string.IsNullOrWhiteSpace(dto.Location)) profile.Location = dto.Location;
        if (!string.IsNullOrWhiteSpace(dto.Bio)) profile.Bio = dto.Bio;
        if (!string.IsNullOrWhiteSpace(dto.Gender)) profile.Gender = dto.Gender;
        if (dto.DateOfBirth.HasValue) profile.DateOfBirth = dto.DateOfBirth;

        // ── Profile picture
        if (dto.ProfilePicture is not null && dto.ProfilePicture.Length > 0)
        {
            if (!string.IsNullOrEmpty(profile.ProfilePicture))
                await _fileStorage.DeleteFileAsync(profile.ProfilePicture, ct);

            var relativePath = await _fileStorage.SaveFileAsync(dto.ProfilePicture, "profiles", ct);
            profile.ProfilePicture = relativePath;
        }

        _uow.AdminProfiles.Update(profile);
        await _uow.SaveChangesAsync(ct);

        return _responseHandler.Success(profile);
    }

    public async Task<Response<NotificationSetting>> UpdateAdminNotificationSettingAsync(Guid userId, AdminNotificationSettingDto dto, CancellationToken ct = default)
    {
        var settings = await _uow.NotificationSettings.GetByUserIdAsync(userId, ct);
        if (settings is null)
            return _responseHandler.NotFound<NotificationSetting>("Notification settings not found");

        settings.EmailNotification = dto.EmailNotification;
        settings.CourseEnrollmentConfirmation = dto.CourseEnrollmentConfirmation;
        settings.GradePublished = dto.GradePublished;
        settings.AssignmentReminder = dto.AssignmentReminders;
        settings.WeeklyActivityDigest = dto.WeeklyActivityDigest;
        settings.ExamNotification = dto.ExamReminders;
        settings.EmergencyAlerts = dto.EmergencyAlerts;
        settings.InAppNotification = dto.InAppNotification;
        settings.LowAttendanceAlert = dto.LowAttendanceAlert;
        settings.FailingGradeWarning = dto.FailingGradeWarning;
        settings.EnrollmentCapacityAlert = dto.EnrollmentCapacityAlert;

        _uow.NotificationSettings.Update(settings);
        await _uow.SaveChangesAsync(ct);

        return _responseHandler.Success(settings);
    }

    public async Task<Response<NotificationSetting>> UpdateAdminNotification_PrefrancesSettingAsync(Guid userId, AdminNotificationPrefrancesDto dto, CancellationToken ct = default)
    {
        var setting = await _uow.NotificationSettings.GetByUserIdAsync(userId, ct);
        if (setting is null)
        {
            setting = new NotificationSetting();
            await _uow.NotificationSettings.AddAsync(setting, ct);
        }

        setting.EmailNotification = dto.EmailNotifications;
        setting.QuizSubmission = dto.QuizSubmissions;
        setting.NewStudentEnrollmentEmail = dto.StudentEnrollments;
        setting.PlatformUpdates = dto.SystemUpdates;

        _uow.NotificationSettings.Update(setting);
        await _uow.SaveChangesAsync(ct);

        var userSettings = await _uow.NotificationSettings.GetByUserIdAsync(userId, ct);
        return _responseHandler.Success(userSettings!);
    }

    public async Task<Response<ApplicationUser>> UpdateGeneralSettingAsync(Guid userId, GeneralSettingDto dto, CancellationToken ct = default)
    {
        var setting = await _uow.AppUserRepository.GetByIdAsync(userId, ct);
        if (setting is null)
        {
            setting = new ApplicationUser();
            await _uow.AppUserRepository.AddAsync(setting, ct);
        }

        setting.Language = dto.DefaultLanguage;
        setting.TimeZone = dto.DefaultTimeZone;
        setting.UpdatedAt = DateTime.UtcNow;

        _uow.AppUserRepository.Update(setting);
        await _uow.SaveChangesAsync(ct);

        return _responseHandler.Success(setting);
    }

    public async Task<Response<AcademicSetting>> UpdateAcademicSettingAsync(Guid userId, AcademicSettingDto dto, CancellationToken ct = default)
    {
        var setting = await _uow.AcademicSettings.GetAcademicSettingwithGradesAsync(ct);
        if (setting is null)
        {
            setting = new AcademicSetting();
            await _uow.AcademicSettings.AddAsync(setting, ct);
            await _uow.SaveChangesAsync(ct);
        }

        setting.AllowInstructorsToCreateCourses = dto.AllowInstructorsToCreateCourses;
        setting.MaxCourseDurationWeeks = dto.MaxCourseDurationWeeks;
        setting.MinCourseDurationWeeks = dto.MinCourseDurationWeeks;

        var incoming = dto.GradeRanges ?? new List<GradeRangeDto>();
        var deletedRanges = setting.GradeRanges.Where(e => incoming.All(i => i.Id != e.Id)).ToList();
        foreach (var deleted in deletedRanges) _uow.GradeRanges.Remove(deleted);

        foreach (var rangeDto in incoming)
        {
            var existing = setting.GradeRanges.FirstOrDefault(r => r.Id == rangeDto.Id);
            if (existing is not null)
            {
                existing.Letter = rangeDto.Letter;
                existing.MinScore = rangeDto.MinScore;
                existing.MaxScore = rangeDto.MaxScore;
            }
            else
            {
                setting.GradeRanges.Add(new GradeRange
                {
                    AcademicSettingId = setting.Id,
                    Letter = rangeDto.Letter,
                    MinScore = rangeDto.MinScore,
                    MaxScore = rangeDto.MaxScore
                });
            }
        }

        setting.AutoPublishResults = dto.AutoPublishResults;
        setting.ResultReleaseDelayDays = dto.ResultReleaseDelayDays;
        setting.GradeAppealPeriodDays = dto.GradeAppealPeriodDays;
        setting.UpdatedAt = DateTime.UtcNow;

        _uow.AcademicSettings.Update(setting);
        await _uow.SaveChangesAsync(ct);

        return _responseHandler.Success(setting);
    }

    

   

}

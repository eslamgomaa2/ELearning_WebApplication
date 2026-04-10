using E_learning.Core.Entities.Identity;
using E_Learning.Core.Base;
using E_Learning.Core.Entities.Notifications;
using E_Learning.Core.Entities.Profiles;
using E_Learning.Core.Repository;
using E_Learning.Service.DTOs.Profiles.Student;
using E_Learning.Service.Services.Profiles.FileStorageService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace E_Learning.Service.Services.Profiles.StudentSetting
{
    public class StudentService : IStudentService
    {
        private readonly IUnitOfWork _uow;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IFileStorage _fileStorage;
        private readonly ResponseHandler _responseHandler;

        public StudentService(
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
        public async Task<Response<StudentProfile>> GetAllSettingsAsync(Guid userId)
        {
            var profile = await _uow.StudentProfiles.GetStudentProfileWithUserByUserIdAsync(userId);

            if (profile is null)
                return _responseHandler.NotFound<StudentProfile>("Profile not found");

            return _responseHandler.Success(profile);
        }

        public async Task<Response<StudentProfile>> UpdateStudentInformationAsync( Guid userId,  UpdateStudentProfileDto dto, CancellationToken ct)
        {
            var profile = await _uow.StudentProfiles.GetStudentProfileWithUserByUserIdAsync(userId);

            if (profile is null)
                return _responseHandler.NotFound<StudentProfile>("Profile not found");

            
            if (!string.IsNullOrWhiteSpace(dto.FullName))
                profile.AppUser.FullName = dto.FullName;

           

            if (!string.IsNullOrWhiteSpace(dto.phoneNumber))
                profile.AppUser.PhoneNumber = dto.phoneNumber;

           

            if (dto.DateOfBirth.HasValue)
                profile.DateOfBirth = dto.DateOfBirth.Value;

            if (dto.ProfilePicture is not null && dto.ProfilePicture.Length > 0)
            {
                if (!string.IsNullOrEmpty(profile.ProfilePicture))
                    await _fileStorage.DeleteFileAsync(profile.ProfilePicture,ct);

                var relativePath = await _fileStorage.SaveFileAsync(dto.ProfilePicture, "profiles",ct);
                profile.ProfilePicture = relativePath;
            }

            _uow.StudentProfiles.Update(profile);
            await _uow.SaveChangesAsync();

            return _responseHandler.Success(profile);
        }

        
        public async Task<Response<EmailSecurityDto>> GetEmailSecurityAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user is null)
                return _responseHandler.NotFound<EmailSecurityDto>("User not found");

            return _responseHandler.Success(new EmailSecurityDto
            {
                Email = user.Email ?? string.Empty,
                IsVerified = user.EmailConfirmed
            });
        }

        
        public async Task<Response<NotificationSetting>> UpdateNotificationSettingAsync(
            Guid userId,
            StudentNotificationSettingDto dto,CancellationToken ct)
        {
            var settings = await _uow.NotificationSettings.GetByUserIdAsync(userId,ct);

            if (settings is null)
                return _responseHandler.NotFound<NotificationSetting>(
                    "Notification settings not found");

           
            settings.CourseAnnouncement = dto.CourseAnnouncement;
            settings.AssignmentReminder = dto.AssignmentReminder;
            settings. ExamNotification= dto.ExamNotification;
            settings.PlatformUpdates = dto.PlatformUpdates;
            settings.InAppNotification = dto.In_AppNotification;
            settings.EmailNotification = dto.EmailNotification;

            _uow.NotificationSettings.Update(settings);
            await _uow.SaveChangesAsync();

            return _responseHandler.Success(settings);
        }


        public async Task<Response<PrivacySettingsDto>> UpdatePrivacySettingsAsync(
     Guid userId,
     PrivacySettingsDto dto)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user is null)
                return _responseHandler.NotFound<PrivacySettingsDto>("User not found");

            
            user.ProfileVisibility = dto.ProfileVisibility;
            user.ShowProgressToOthers = dto.ShowProgressToOthers;
            user.UpdatedAt = DateTime.UtcNow;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return _responseHandler.BadRequest<PrivacySettingsDto>(
                    $"Failed to update privacy settings: {errors}");
            }

            return _responseHandler.Success(new PrivacySettingsDto
            {
                ProfileVisibility = user.ProfileVisibility,
                ShowProgressToOthers = user.ShowProgressToOthers
            });
        }

        
        public async Task<Response<LearningPrefrancesDto>> UpdateLearningPrefrancesAsync(
            Guid userId,
            LearningPrefrancesDto dto)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user is null)
                return _responseHandler.NotFound<LearningPrefrancesDto>("User not found");

            user.Language = dto.Language;
            user.TimeZone = dto.TimeZone;
            user.UpdatedAt = DateTime.UtcNow;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return _responseHandler.BadRequest<LearningPrefrancesDto>(
                    $"Failed to update learning preferences: {errors}");
            }

            return _responseHandler.Success(new LearningPrefrancesDto
            {
                Language = user.Language,
                TimeZone = user.TimeZone
            });

        }

        public async Task<Response<UserDataExportDto>> RequestDataExportAsync(Guid userId)
        {
            
            var profile = await _uow.StudentProfiles.GetStudentProfileAsync(userId);
            if (profile is null)
                return _responseHandler.NotFound<UserDataExportDto>("Profile not found");

            var courses = await _uow.Enrollments.GetByStudentIdAsync(userId);

            var notifications = await _uow.Notifications.GetNotificationByguid(userId);

            // ── 4. FIX: GetPublicUrl is sync — no need for GetFileUrlAsync ─────────
            var profilePictureUrl = string.IsNullOrEmpty(profile.ProfilePicture)
                ? null
                : _fileStorage.GetPublicUrl(profile.ProfilePicture);

            return _responseHandler.Success(new UserDataExportDto
            {
                ExportedAt = DateTime.UtcNow,

                Profile = new ProfileExportDto
                {
                    FullName = profile.AppUser.FullName,
                    Email = profile.AppUser.Email ?? string.Empty,
                    DateOfBirth = profile.DateOfBirth,
                    Location = profile.Location,
                    Language=profile.AppUser.Language,
                    TimeZone= profile.AppUser.TimeZone

                },

                Courses = courses?.Select(c => new EnrollmentExportDto
                {
                   
                    CourseTitle = c.Course.Title,
                    EnrolledAt = c.EnrolledAt,
                   


                }).ToList() ?? new List<EnrollmentExportDto>(),

                ActivityHistory = notifications?.Select(n => new ActivityExportDto
                {
                    Type = n.Type,
                    Body = n.Body,
                    CreatedAt = n.CreatedAt,
                    Title = n.Title,
                    IsRead = n.IsRead
                }).ToList() ?? new List<ActivityExportDto>()
            });
        }

        public async Task<Response<IReadOnlyList<StudentProfile>>> GetAllStudentsAsync()
        {
            var students = await _uow.StudentProfiles.GetAllAsync();
            if (students is null || !students.Any())
            {
                return _responseHandler.NotFound<IReadOnlyList<StudentProfile>>("No students found");
            }

            return _responseHandler.Success(students);
        }

        public async Task<Response<StudentProfile>> GetStudentProfileByUserIdAsync(Guid id)
        {
            var student = await _uow.StudentProfiles.GetStudentProfileWithUserByUserIdAsync(id);

            if (student is null)
            {
                return _responseHandler.NotFound<StudentProfile>("Student profile not found");
            }

            return _responseHandler.Success(student);
        }

      

       
    }

}
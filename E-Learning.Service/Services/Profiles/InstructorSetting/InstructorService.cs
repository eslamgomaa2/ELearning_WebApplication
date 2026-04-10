using E_learning.Core.Entities.Identity;
using E_Learning.Core.Base;
using E_Learning.Core.Entities.Notifications;
using E_Learning.Core.Entities.Profiles;
using E_Learning.Core.Interfaces;
using E_Learning.Core.Repository;
using E_Learning.Service.DTOs.Profiles;
using E_Learning.Service.DTOs.Profiles.Instructor;
using E_Learning.Service.Services.Profiles.FileStorageService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace E_Learning.Service.Services.Profiles.InstructorSetting
{
    public class InstructorService : IInstructorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ResponseHandler _responseHandler;
        private readonly IFileStorage _fileStorage;

        public InstructorService(
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager,
            IFileStorage fileStorage,
            ResponseHandler responseHandler)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _fileStorage = fileStorage;
            _responseHandler = responseHandler;
        }

        
        public async Task<Response<NotificationSetting>> GetAllSettingsAsync(Guid userId, CancellationToken ct)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

         
            if (user is null)
                return _responseHandler.NotFound<NotificationSetting>("User not found");

            var settings = await _unitOfWork.NotificationSettings.GetByUserIdAsync(userId,ct);

            if (settings is null)
                return _responseHandler.NotFound<NotificationSetting>("Profile not found");

            return _responseHandler.Success(settings);
        }
        public async Task<Response<InstructorProfile>> UpdatePersonalDetailsAsync( Guid userId,UpdateInstructorProfileDto dto, CancellationToken ct)
        {
            var profile = await _unitOfWork.InstructorProfiles.GetProfileByUserIdAsync(userId);

            
            if (profile is null)
                return _responseHandler.NotFound<InstructorProfile>("Profile not found");

            
            if (!string.IsNullOrWhiteSpace(dto.FullName))
                profile.AppUser.FullName = dto.FullName;

           
            if (!string.IsNullOrWhiteSpace(dto.phoneNumber))
                profile.phoneNumber = dto.phoneNumber;

            if (dto.Bio is not null)
                profile.Bio = dto.Bio;

            _unitOfWork.InstructorProfiles.Update(profile);
            await _unitOfWork.SaveChangesAsync();

            return _responseHandler.Success(profile);
        }

        
        public async Task<Response<NotificationSetting>> UpdateInstructorNotificationsAsync( Guid userId, InstructorNotificationSettingsDto dto,CancellationToken ct)
        {
            var settings = await _unitOfWork.NotificationSettings.GetByUserIdAsync(userId,ct);

            
            if (settings is null)
                return _responseHandler.NotFound<NotificationSetting>("No notification settings found");

            settings.NewStudentEnrollmentInApp = dto.NewStudentEnrollmentInApp;
            settings.NewStudentEnrollmentEmail = dto.NewStudentEnrollmentEmail;
            settings.ExamSubmissionInApp = dto.ExamSubmissionInApp;
            settings.ExamSubmissionEmail = dto.ExamSubmissionEmail;

            _unitOfWork.NotificationSettings.Update(settings);
            await _unitOfWork.SaveChangesAsync();

            return _responseHandler.Success(settings);
        }

        
       
    }
}
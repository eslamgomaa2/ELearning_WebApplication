using E_learning.Core.Entities.Identity;
using E_Learning.Core.Base;
using E_Learning.Core.Repository;
using E_Learning.Service.DTOs.Profiles;
using E_Learning.Service.Services.Profiles.FileStorageService;
using E_Learning.Service.Services.Profiles.GenericProfileSettingServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace E_Learning.Service.Services.Profiles.GenericProfileSetting
{
    public class GenericProfileSettingServices : IGenericProfileSettingServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ResponseHandler _responseHandler;
        private readonly IFileStorage _fileStorage;

        public GenericProfileSettingServices(IFileStorage fileStorage, ResponseHandler responseHandler, UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork)
        {
            _fileStorage = fileStorage;
            _responseHandler = responseHandler;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<string>> UpdatePasswordAsync(Guid userId, ChangePasswordDto dto)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());


            if (user is null)
                return _responseHandler.NotFound<string>("User not found");

            var passwordValid = await _userManager.CheckPasswordAsync(user, dto.CurrentPassword);
            if (!passwordValid)
                return _responseHandler.BadRequest<string>("Current password is incorrect");
            if (dto.NewPassword != dto.ConfirmPassword)
            {
                return _responseHandler.BadRequest<string>("New password and confirmation do not match");
            }
            var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return _responseHandler.BadRequest<string>($"Failed to change password: {errors}");
            }

            return _responseHandler.Success("Password changed successfully");
        }


        public async Task<Response<string>> UploadProfilePictureAsync(Guid userId, IFormFile file,CancellationToken ct)
        {
            var profile = await _unitOfWork.InstructorProfiles.GetProfileByUserIdAsync(userId);

            if (profile is null)
                return _responseHandler.NotFound<string>("Profile not found");


            if (!string.IsNullOrEmpty(profile.ProfilePicture))
                await _fileStorage.DeleteFileAsync(profile.ProfilePicture,ct);

            var relativePath = await _fileStorage.SaveFileAsync(file, "profiles",ct);
            var publicUrl = _fileStorage.GetPublicUrl(relativePath);

            profile.ProfilePicture = relativePath;
            _unitOfWork.InstructorProfiles.Update(profile);
            await _unitOfWork.SaveChangesAsync();
            return _responseHandler.Success(publicUrl);
        }


        public async Task<Response<string>> DeleteProfilePictureAsync(Guid userId,CancellationToken ct)
        {
            var profile = await _unitOfWork.InstructorProfiles.GetProfileByUserIdAsync(userId);

            if (profile is null)
                return _responseHandler.NotFound<string>("Profile not found");


            if (string.IsNullOrEmpty(profile.ProfilePicture))
                return _responseHandler.BadRequest<string>("No profile picture to remove");

            await _fileStorage.DeleteFileAsync(profile.ProfilePicture,ct);

            profile.ProfilePicture = null;
            _unitOfWork.InstructorProfiles.Update(profile);
            await _unitOfWork.SaveChangesAsync();


            return _responseHandler.Success("Profile picture removed successfully");
        }

       
    }
}

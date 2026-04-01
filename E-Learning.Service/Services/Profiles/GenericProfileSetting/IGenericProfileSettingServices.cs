using E_Learning.Core.Base;
using E_Learning.Service.DTOs.Profiles;
using Microsoft.AspNetCore.Http;

namespace E_Learning.Service.Services.Profiles.GenericProfileSettingServices
{
    public interface IGenericProfileSettingServices
    {
        Task<Response<string>> UpdatePasswordAsync(Guid userId, ChangePasswordDto dto);
        Task<Response<string>> UploadProfilePictureAsync(Guid userId, IFormFile file,CancellationToken ct);
        Task<Response<string>> DeleteProfilePictureAsync(Guid userId,CancellationToken ct);
    }
}

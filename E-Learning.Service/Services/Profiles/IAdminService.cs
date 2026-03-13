using E_Learning.Core.Base;
using E_Learning.Core.Entities.Profiles;
using E_Learning.Service.DTOs.Profiles.Admin;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace E_Learning.Service.Services.Profiles
{
    public interface IAdminService
    {
        Task<Response<AdminProfileResponseDto>> GetAdminProfileByUserId(Guid userId);
        Task<Response<AdminProfileResponseDto>> CreateAdminProfile(Guid userId, CreateAdminProfileDTo dto, CancellationToken ct = default);
        Task<Response<AdminProfileResponseDto>> UpdateAdminProfile(Guid userId, CreateAdminProfileDTo dto);
        Task<Response<bool>> AdminProfileExists(Guid userId);
        Task<Response<IEnumerable<AdminProfileResponseDto>>> GetAllAdmins();
        Task<Response<AdminProfileResponseDto>> DeleteAdminProfile(Guid userId);
        Task<Response<string>> UploadProfilePicture(Guid userId, IFormFile file);
    }
}
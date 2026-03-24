using E_Learning.Core.Base;
using E_Learning.Core.Entities.Profiles;
using E_Learning.Service.DTOs.Profiles.Admin;
using E_Learning.Service.DTOs.Profiles.Instructor;
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
        Task<Response<UserResponseDto>> CreateUserProfile( CreateAdminProfileDto dto, CancellationToken ct = default);
       // Task<Response<InstructorProfileResponseDto>> CreateInstructorProfile(CreateInstructorProfileDto dto, CancellationToken ct = default);
        Task<Response<AdminProfileResponseDto>> UpdateAdminProfile(Guid userId, UpdateAdminProfileDto dto);
        Task<Response<bool>> AdminProfileExists(Guid userId);
        Task<Response<IEnumerable<AdminProfileResponseDto>>> GetAllAdmins();
        Task<Response<AdminProfileResponseDto>> DeleteAdminProfile(Guid userId);
      
    }
}
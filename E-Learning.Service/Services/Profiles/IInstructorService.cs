using E_Learning.Core.Base;
using E_Learning.Core.Entities.Profiles;
using E_Learning.Service.DTOs.Profiles.Instructor;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace E_Learning.Service.Services.Profiles
{
    public interface IInstructorService
    {
        Task<Response<InstructorProfileResponseDto>> GetInstructorProfileByUserId(Guid userId);
        //Task<Response<InstructorProfileResponseDto>> CreateInstructorProfile(CreateInstructorProfileDto dto, CancellationToken ct = default);
        Task<Response<InstructorProfileResponseDto>> UpdateInstructorProfile(Guid userId, CreateInstructorProfileDto dto);
        Task<Response<bool>> InstructorProfileExists(Guid userId);
        Task<Response<IEnumerable<InstructorProfileResponseDto>>> GetAllInstructors();
        Task<Response<InstructorProfileResponseDto>> DeleteInstructorProfile(Guid userId);
      
    }
}
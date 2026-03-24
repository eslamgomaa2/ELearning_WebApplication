using E_Learning.Core.Base;
using E_Learning.Core.Entities.Profiles;
using E_Learning.Service.DTOs.Profiles;
using E_Learning.Service.DTOs.Profiles.Instructor;
using E_Learning.Service.DTOs.Profiles.Student;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace E_Learning.Service.Services.Profiles
{
    public interface IStudentService
    {
        Task<Response<StudentProfileResponseDto>> GetStudentProfileByUserId(Guid userId);
       // Task<Response<StudentProfileResponseDto>> CreateStudentProfile( UpdateStudentProfileDto dto, CancellationToken ct = default);
        Task<Response<StudentProfileResponseDto>> UpdateStudentProfile(Guid userId, UpdateStudentProfileDto dto);
        Task<Response<bool>> StudentProfileExists(Guid userId);
        Task<Response<IEnumerable<StudentProfileResponseDto>>> GetAllStudents();
        Task<Response<StudentProfileResponseDto>> DeleteStudentProfile(Guid userId);
        Task<Response<string>> ChangePasswordAsync(Guid userId, ChangePasswordDto dto);

    }
}
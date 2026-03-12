using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using E_Learning.Core.Base;
using E_Learning.Service.DTOs.LiveSessionDto;

namespace E_Learning.Service.Services.LiveSessionServices
{
    public interface ILiveSessionService
    {
        Task<Response<IReadOnlyList<LiveSessionResponseDto>>> GetAllAsync(string? search, int? status, CancellationToken ct = default);
        
        Task<Response<LiveSessionResponseDto>> GetByIdAsync(int id, CancellationToken ct = default);
        
        Task<Response<string>> CreateAsync(CreateLiveSessionDto dto, CancellationToken ct = default);
        
        Task<Response<string>> UpdateAsync(int id, UpdateLiveSessionDto dto, CancellationToken ct = default);
        
        Task<Response<string>> DeleteAsync(int id, CancellationToken ct = default);
    }
}
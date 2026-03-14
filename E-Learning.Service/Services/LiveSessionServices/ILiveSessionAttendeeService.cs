using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using E_Learning.Core.Base;
using E_Learning.Service.DTOs.LiveSessionDto;

namespace E_Learning.Service.Services.LiveSessionServices
{
    public interface ILiveSessionAttendeeService
    {
        Task<Response<AttendeeResponseDto>> LogAttendanceAsync(LogAttendanceDto dto, CancellationToken ct = default);
        Task<Response<IReadOnlyList<AttendeeResponseDto>>> GetAttendeesBySessionIdAsync(int sessionId, CancellationToken ct = default);
    }
}
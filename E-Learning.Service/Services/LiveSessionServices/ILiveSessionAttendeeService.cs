using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using E_Learning.Core.Base;
using E_Learning.Service.DTOs.LiveSessionDto;

namespace E_Learning.Service.Services.LiveSessionServices
{
    public interface ILiveSessionAttendeeService
    {
        // تسجيل دخول الطالب للجلسة
        Task<Response<AttendeeResponseDto>> LogAttendanceAsync(LogAttendanceDto dto, CancellationToken ct = default);

        // جلب كل الطلاب المشاركين في جلسة معينة
        Task<Response<IReadOnlyList<AttendeeResponseDto>>> GetAttendeesBySessionIdAsync(int sessionId, CancellationToken ct = default);

        // خروج الطالب من الجلسة (تحديث LeftAt)
        Task<Response<AttendeeResponseDto>> LeaveSession(LeaveSessionDto dto, CancellationToken ct = default);
    }
}
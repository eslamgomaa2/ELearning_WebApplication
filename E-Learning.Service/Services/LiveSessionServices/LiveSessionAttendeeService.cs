using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using E_Learning.Core.Base;
using E_Learning.Core.Entities.LiveSessions;
using E_Learning.Core.Enums;
using E_Learning.Core.Repository;
using E_Learning.Service.DTOs.LiveSessionDto;
using E_Learning.Service.Hubs;
using E_Learning.Service.Services.Profiles.StudentSetting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace E_Learning.Service.Services.LiveSessionServices
{
    public class LiveSessionAttendeeService : ILiveSessionAttendeeService
    {
        private readonly IUnitOfWork _uow;
        private readonly ResponseHandler _responseHandler;
        private readonly IMapper _mapper;
        private readonly IHubContext<LiveSessionHub> _hubContext;
        private readonly IStudentService _studentService; 

        public LiveSessionAttendeeService(
            IUnitOfWork uow,
            ResponseHandler responseHandler,
            IMapper mapper,
            IHubContext<LiveSessionHub> hubContext,
            IStudentService studentService) 
        {
            _uow = uow;
            _responseHandler = responseHandler;
            _mapper = mapper;
            _hubContext = hubContext;
            _studentService = studentService;
        }

    public async Task<Response<AttendeeResponseDto>> LogAttendanceAsync(LogAttendanceDto dto, CancellationToken ct = default)
{
    // 1️⃣ جلب الجلسة والتحقق من وجودها
    var session = await _uow.LiveSessions.GetByIdAsync(dto.SessionId, ct);
    if (session == null)
        return _responseHandler.NotFound<AttendeeResponseDto>("Live session not found.");

    // 2️⃣ التحقق من أن الجلسة حالتها Live
    if (session.Status != LiveSessionStatus.Live)
        return _responseHandler.BadRequest<AttendeeResponseDto>(
            $"Cannot join session. The session is currently '{session.Status}'.");

    // 3️⃣ التحقق إذا الطالب دخل قبل هيك
    var alreadyLogged = await _uow.LiveSessionAttendees
        .IsStudentEnrolledAsync(dto.SessionId, dto.StudentId, ct);

    if (alreadyLogged)
        return _responseHandler.BadRequest<AttendeeResponseDto>("Student has already joined this session.");

    // 4️⃣ تسجيل الطالب
    var attendee = _mapper.Map<LiveSessionAttendee>(dto);
    await _uow.LiveSessionAttendees.AddAsync(attendee, ct);
    await _uow.SaveChangesAsync(ct);

   // 5️⃣ جلب الحضور الحالي للطالب فقط
var currentAttendee = (await _uow.LiveSessionAttendees
    .GetAttendeesBySessionIdAsync(dto.SessionId, ct))
    .FirstOrDefault(x => x.StudentId == dto.StudentId);

if (currentAttendee == null)
    return _responseHandler.NotFound<AttendeeResponseDto>("Error retrieving attendance data.");

// 6️⃣ Mapping للـ DTO مباشرة على العنصر وليس الـ IEnumerable
var result = _mapper.Map<AttendeeResponseDto>(currentAttendee);

// 7️⃣ إضافة بيانات الـ Profile إذا موجودة
var studentProfileResponse = await _studentService.GetStudentProfileByUserIdAsync(dto.StudentId);

if (studentProfileResponse.Data != null)
{
    result.Student.ProfilePicture = studentProfileResponse.Data.ProfilePicture;
    result.Student.Location = studentProfileResponse.Data.Location;
    // أي بيانات إضافية من Profile
}

// 8️⃣ إشعار عبر SignalR
await _hubContext.Clients
    .Group(dto.SessionId.ToString())
    .SendAsync("OnStudentJoined", result);

return _responseHandler.Success(result);}

        public async Task<Response<IReadOnlyList<AttendeeResponseDto>>> GetAttendeesBySessionIdAsync(int sessionId, CancellationToken ct)
        {
            var attendees = await _uow.LiveSessionAttendees
                .GetAttendeesBySessionIdAsync(sessionId, ct);

            if (attendees == null || !attendees.Any())
                return _responseHandler.Success<IReadOnlyList<AttendeeResponseDto>>(new List<AttendeeResponseDto>());

            var allStudentsResponse = await _studentService.GetAllStudentsAsync();
            var allStudents = allStudentsResponse.Data;

            var result = _mapper.Map<IReadOnlyList<AttendeeResponseDto>>(attendees);

            foreach (var attendeeDto in result)
            {
                var studentInfo = allStudents?.FirstOrDefault(s => s.AppUserId == attendeeDto.Student.Id);
                if (studentInfo != null)
                {
                    attendeeDto.Student.ProfilePicture = studentInfo.ProfilePicture;
                    attendeeDto.Student.Location = studentInfo.Location;
                }
            }

            return _responseHandler.Success(result);
        }

        public async Task<Response<AttendeeResponseDto>> LeaveSession(LeaveSessionDto dto, CancellationToken ct = default)
        {
             // 1️⃣ جلب الطالب الحالي في الجلسة (LeftAt == null)
    var attendees = await _uow.LiveSessionAttendees.GetAttendeesBySessionIdAsync(dto.SessionId, ct);
var existing = attendees.FirstOrDefault(x => x.StudentId == dto.StudentId && x.LeftAt == null);

    if (existing == null)
        return _responseHandler.BadRequest<AttendeeResponseDto>("Student is not in the session or already left.");

    // 2️⃣ تعديل LeftAt وحساب المدة بالثواني
    existing.LeftAt = DateTime.UtcNow;
    existing.DurationSeconds = (int)(existing.LeftAt.Value - existing.JoinedAt).TotalSeconds;

    // 3️⃣ حفظ التعديلات
    _uow.LiveSessionAttendees.Update(existing); // لازم تتأكد ان الريبو فيه Update أو اعمل SaveChanges مباشرة
    await _uow.SaveChangesAsync(ct);

    // 4️⃣ جلب بيانات الطالب لتحديث الـDTO
    var studentProfileResponse = await _studentService.GetStudentProfileByUserIdAsync(dto.StudentId);
    var result = _mapper.Map<AttendeeResponseDto>(existing);

    if (studentProfileResponse.Data != null)
    {
        result.Student.ProfilePicture = studentProfileResponse.Data.ProfilePicture;
        result.Student.Location = studentProfileResponse.Data.Location;
    }

    // 5️⃣ إرسال إشعار عبر SignalR
    await _hubContext.Clients
        .Group(dto.SessionId.ToString())
        .SendAsync("OnStudentLeft", result);

    return _responseHandler.Success(result);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using E_Learning.Core.Base;
using E_Learning.Core.Entities.LiveSessions;
using E_Learning.Core.Repository;
using E_Learning.Service.DTOs.LiveSessionDto;
using E_Learning.Service.DTOs.LiveSessionDto.E_Learning.Service.DTOs.LiveSessionDto;
using E_Learning.Service.Hubs;
using E_Learning.Service.Services.Profiles; 
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
            var alreadyLogged = await _uow.LiveSessionAttendees
                .IsStudentEnrolledAsync(dto.SessionId, dto.StudentId, ct);

            if (alreadyLogged)
                return _responseHandler.BadRequest<AttendeeResponseDto>("Student has already joined this session.");

            var attendee = _mapper.Map<LiveSessionAttendee>(dto);
            await _uow.LiveSessionAttendees.AddAsync(attendee, ct);
            await _uow.SaveChangesAsync(ct);

            var attendeesList = await _uow.LiveSessionAttendees
                .GetAttendeesBySessionIdAsync(dto.SessionId, ct);

            var currentAttendee = attendeesList
                .FirstOrDefault(x => x.StudentId == dto.StudentId);

            if (currentAttendee == null)
                return _responseHandler.NotFound<AttendeeResponseDto>("Error retrieving attendance data.");

            var studentProfileResponse = await _studentService.GetStudentProfileByUserId(dto.StudentId);
            
            var result = _mapper.Map<AttendeeResponseDto>(currentAttendee);

            if (studentProfileResponse.Data != null)
            {
                result.Student.ProfilePicture = studentProfileResponse.Data.ProfilePicture;
                result.Student.Location = studentProfileResponse.Data.Location;
            }

            await _hubContext.Clients
                .Group(dto.SessionId.ToString())
                .SendAsync("OnStudentJoined", result);

            return _responseHandler.Success(result);
        }

        public async Task<Response<IReadOnlyList<AttendeeResponseDto>>> GetAttendeesBySessionIdAsync(int sessionId, CancellationToken ct)
        {
            var attendees = await _uow.LiveSessionAttendees
                .GetAttendeesBySessionIdAsync(sessionId, ct);

            if (attendees == null || !attendees.Any())
                return _responseHandler.Success<IReadOnlyList<AttendeeResponseDto>>(new List<AttendeeResponseDto>());

            var allStudentsResponse = await _studentService.GetAllStudents();
            var allStudents = allStudentsResponse.Data;

            var result = _mapper.Map<IReadOnlyList<AttendeeResponseDto>>(attendees);

            foreach (var attendeeDto in result)
            {
                var studentInfo = allStudents?.FirstOrDefault(s => s.Id == attendeeDto.Student.Id);
                if (studentInfo != null)
                {
                    attendeeDto.Student.ProfilePicture = studentInfo.ProfilePicture;
                    attendeeDto.Student.Location = studentInfo.Location;
                }
            }

            return _responseHandler.Success(result);
        }
    }
}
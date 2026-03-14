using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using E_Learning.Core.Base;
using E_Learning.Core.Entities.LiveSessions;
using E_Learning.Core.Repository;
using E_Learning.Service.DTOs.LiveSessionDto;

namespace E_Learning.Service.Services.LiveSessionServices
{
    public class LiveSessionAttendeeService : ILiveSessionAttendeeService
    {
        private readonly IUnitOfWork _uow;
        private readonly ResponseHandler _responseHandler;
        private readonly IMapper _mapper;


        public LiveSessionAttendeeService(IUnitOfWork uow, ResponseHandler responseHandler, IMapper mapper)
        {
            _uow = uow;
            _responseHandler = responseHandler;
            _mapper = mapper;
        }

        public async Task<Response<AttendeeResponseDto>> LogAttendanceAsync(LogAttendanceDto dto, CancellationToken ct = default)
        {
            var alreadyLogged = await _uow.LiveSessionAttendees.IsStudentEnrolledAsync(dto.SessionId, dto.StudentId, ct);

            if (alreadyLogged)
                return _responseHandler.BadRequest<AttendeeResponseDto>("Student has already joined this session.");

            var attendee = _mapper.Map<LiveSessionAttendee>(dto);
            await _uow.LiveSessionAttendees.AddAsync(attendee, ct);
            await _uow.SaveChangesAsync(ct);

            var attendeesList = await _uow.LiveSessionAttendees.GetAttendeesBySessionIdAsync(dto.SessionId, ct);
            var currentAttendee = attendeesList.FirstOrDefault(x => x.StudentId == dto.StudentId);

            if (currentAttendee == null)
                return _responseHandler.NotFound<AttendeeResponseDto>("Error retrieving attendance data.");

            var result = _mapper.Map<AttendeeResponseDto>(currentAttendee);

            return _responseHandler.Success(result);
        }

        public async Task<Response<IReadOnlyList<AttendeeResponseDto>>> GetAttendeesBySessionIdAsync(int sessionId, CancellationToken ct)
        {
            var attendees = await _uow.LiveSessionAttendees.GetAttendeesBySessionIdAsync(sessionId, ct);

            var result = _mapper.Map<IReadOnlyList<AttendeeResponseDto>>(attendees);
            return _responseHandler.Success(result);

        }
    }
}

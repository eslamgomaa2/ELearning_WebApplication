using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using E_Learning.Core.Base;
using E_Learning.Core.Entities.LiveSessions;
using E_Learning.Core.Enums;
using E_Learning.Core.Interfaces.Services.Courses;
using E_Learning.Core.Repository;
using E_Learning.Service.DTOs.LiveSessionDto;
using E_Learning.Service.Hubs;
using E_Learning.Service.Services.Profiles;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace E_Learning.Service.Services.LiveSessionServices
{
    public class LiveSessionService : ILiveSessionService
    {
        private readonly IUnitOfWork _uow;
        private readonly ResponseHandler _responseHandler;
        private readonly IMapper _mapper;
        private readonly ICourseService _courseService;
        private readonly IInstructorService _instructorService;
        private readonly IHubContext<LiveSessionHub> _hubContext;

        public LiveSessionService(IUnitOfWork uow, ResponseHandler responseHandler, IMapper mapper, ICourseService courseService,
        IInstructorService instructorService, IHubContext<LiveSessionHub> hubContext)
        {
            _uow = uow;
            _responseHandler = responseHandler;
            _mapper = mapper;
            _courseService = courseService;
            _instructorService = instructorService;
            _hubContext = hubContext;
        }

        public async Task<Response<IReadOnlyList<LiveSessionResponseDto>>> GetAllAsync(string? search, string? status, CancellationToken ct = default)
        {
            var query = _uow.LiveSessions.GetTableNoTracking()
                            .Include(x => x.Course)
                            .Include(x => x.Attendees)
                            .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(x => x.Title.Contains(search));

            if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<LiveSessionStatus>(status, true, out var statusEnum))
                query = query.Where(x => x.Status == statusEnum);

            var sessions = await query.ToListAsync(ct);
            if (sessions == null || !sessions.Any())
                return _responseHandler.NotFound<IReadOnlyList<LiveSessionResponseDto>>("No live sessions found.");

            var mappedData = _mapper.Map<List<LiveSessionResponseDto>>(sessions);

            foreach (var dto in mappedData)
            {
                var instructorInfo = await _instructorService.GetInstructorProfileByUserId(dto.userId); 
                if (instructorInfo.Data != null)
                {
                    dto.Instructor = instructorInfo.Data;
                }
            }

            return _responseHandler.Success((IReadOnlyList<LiveSessionResponseDto>)mappedData);
        }

        public async Task<Response<LiveSessionResponseDto>> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var session = await _uow.LiveSessions.GetTableNoTracking()
                .Include(x => x.Course)
                .Include(x => x.Attendees)
                .FirstOrDefaultAsync(x => x.Id == id, ct);

            if (session is null)
                return _responseHandler.NotFound<LiveSessionResponseDto>($"Live Session with ID {id} not found.");

            var dto = _mapper.Map<LiveSessionResponseDto>(session);

            var instructorInfo = await _instructorService.GetInstructorProfileByUserId(session.InstructorId);
            if (instructorInfo.Data != null)
            {
                dto.Instructor = instructorInfo.Data;
            }

            return _responseHandler.Success(dto);
        }

        async Task<Response<LiveSessionResponseDto>> ILiveSessionService.CreateAsync(CreateLiveSessionDto dto, CancellationToken ct)
        {
            var courseResponse = await _courseService.GetCourseByIdAsync(dto.CourseId, ct);
            if (courseResponse.Data == null)
                return _responseHandler.NotFound<LiveSessionResponseDto>("Course not found.");

            var instructorProfile = await _instructorService.GetInstructorProfileByUserId(dto.InstructorId);
            if (instructorProfile.Data == null)
                return _responseHandler.NotFound<LiveSessionResponseDto>("Instructor profile not found.");

            var session = _mapper.Map<LiveSession>(dto);
            await _uow.LiveSessions.AddAsync(session, ct);
            await _uow.SaveChangesAsync(ct);

            var result = await GetByIdAsync(session.Id, ct);

            await _hubContext.Clients.All.SendAsync("OnSessionCreated", result.Data);
            return _responseHandler.Created(result.Data);
        }

        async Task<Response<LiveSessionResponseDto>> ILiveSessionService.UpdateAsync(int id, UpdateLiveSessionDto dto, CancellationToken ct)
        {
            var session = await _uow.LiveSessions.GetByIdAsync(id, ct);
            if (session is null)
                return _responseHandler.NotFound<LiveSessionResponseDto>($"Live Session {id} not found.");

            _mapper.Map(dto, session);
            _uow.LiveSessions.Update(session);
            await _uow.SaveChangesAsync(ct);

            var result = await GetByIdAsync(id, ct);

            await _hubContext.Clients.All.SendAsync("OnSessionUpdated", result.Data);
            return _responseHandler.Success(result.Data);
        }

        async Task<Response<LiveSessionResponseDto>> ILiveSessionService.DeleteAsync(int id, CancellationToken ct)
        {
            var result = await GetByIdAsync(id, ct);
            if (result.Data == null)
                return _responseHandler.NotFound<LiveSessionResponseDto>("Session not found.");

            var entityToDelete = await _uow.LiveSessions.GetByIdAsync(id, ct);
            _uow.LiveSessions.SoftDelete(entityToDelete);
            await _uow.SaveChangesAsync(ct);

            await _hubContext.Clients.All.SendAsync("OnSessionDeleted", id);

            return _responseHandler.Success(result.Data);
        }
    }
}
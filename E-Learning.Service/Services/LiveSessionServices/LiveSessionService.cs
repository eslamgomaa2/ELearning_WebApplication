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
using E_Learning.Service.Services.Profiles;
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

        public LiveSessionService(IUnitOfWork uow, ResponseHandler responseHandler, IMapper mapper, ICourseService courseService,
        IInstructorService instructorService)
        {
            _uow = uow;
            _responseHandler = responseHandler;
            _mapper = mapper;
            _courseService = courseService;
            _instructorService = instructorService;
        }


        public async Task<Response<IReadOnlyList<LiveSessionResponseDto>>> GetAllAsync(string? search, string? status, CancellationToken ct = default)
        {
            var query = _uow.LiveSessions.GetTableNoTracking()
                   .Include(x => x.Course)
                   .Include(x => x.Instructor)
                   .Include(x => x.Attendees)
                   .AsQueryable();

            // 1. Search by title
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x => x.Title.Contains(search));
            }

            // 2. Filter by status (Incoming as string)
            if (!string.IsNullOrWhiteSpace(status))
            {

                if (Enum.TryParse<LiveSessionStatus>(status, true, out var statusEnum))
                {
                    query = query.Where(x => x.Status == statusEnum);
                }
                else
                {
                    return _responseHandler.BadRequest<IReadOnlyList<LiveSessionResponseDto>>($"Invalid status value: {status}. Available statuses are: Scheduled, Live, Completed, Cancelled.");
                }
            }

            var sessions = await query.ToListAsync(ct);

            if (sessions == null || !sessions.Any())
            {
                return _responseHandler.NotFound<IReadOnlyList<LiveSessionResponseDto>>("No live sessions were found matching your criteria.");
            }

            var mappedData = _mapper.Map<IReadOnlyList<LiveSessionResponseDto>>(sessions);

            return _responseHandler.Success(mappedData);
        }

        public async Task<Response<LiveSessionResponseDto>> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var session = await _uow.LiveSessions.GetTableNoTracking()
            .Include(x => x.Course)
            .Include(x => x.Instructor)
            .Include(x => x.Attendees)
            .FirstOrDefaultAsync(x => x.Id == id, ct);

            if (session is null)
                return _responseHandler.NotFound<LiveSessionResponseDto>($"Live Session with ID {id} was not found.");

            var dto = _mapper.Map<LiveSessionResponseDto>(session);
            return _responseHandler.Success(dto);
        }


        async Task<Response<LiveSessionResponseDto>> ILiveSessionService.CreateAsync(CreateLiveSessionDto dto, CancellationToken ct)
        {
            var courseResponse = await _courseService.GetCourseByIdAsync(dto.CourseId, ct);
            if (courseResponse.Data == null)
            {
                return _responseHandler.NotFound<LiveSessionResponseDto>($"Cannot create session: Course with ID {dto.CourseId} not found.");
            }
            var instructorCheck = await _instructorService.InstructorProfileExists(dto.InstructorId);
            if (instructorCheck.Data == false)
            {
                return _responseHandler.NotFound<LiveSessionResponseDto>($"Cannot create session: Instructor with ID {dto.InstructorId} not found.");
            }
            var session = _mapper.Map<LiveSession>(dto);

            await _uow.LiveSessions.AddAsync(session, ct);
            await _uow.SaveChangesAsync(ct);
            var result = await GetByIdAsync(session.Id, ct);

            return _responseHandler.Created(result.Data);
        }

        async Task<Response<LiveSessionResponseDto>> ILiveSessionService.DeleteAsync(int id, CancellationToken ct)
        {
            var session = await _uow.LiveSessions.GetTableNoTracking()
         .Include(x => x.Course)
         .Include(x => x.Instructor)
         .Include(x => x.Attendees)
         .FirstOrDefaultAsync(x => x.Id == id, ct);

            // 2. Check if it exists
            if (session is null)
                return _responseHandler.NotFound<LiveSessionResponseDto>($"Live Session with ID {id} was not found.");

            var deletedData = _mapper.Map<LiveSessionResponseDto>(session);

            var entityToDelete = await _uow.LiveSessions.GetByIdAsync(id, ct);
            _uow.LiveSessions.SoftDelete(entityToDelete);
            await _uow.SaveChangesAsync(ct);

            return _responseHandler.Success(deletedData);
        }

        async Task<Response<LiveSessionResponseDto>> ILiveSessionService.UpdateAsync(int id, UpdateLiveSessionDto dto, CancellationToken ct)
        {
            var session = await _uow.LiveSessions.GetByIdAsync(id, ct);
            if (session is null)
                return _responseHandler.NotFound<LiveSessionResponseDto>($"Live Session {id} not found.");

            if (dto.CourseId != session.CourseId)
            {
                var courseCheck = await _courseService.GetCourseByIdAsync(dto.CourseId, ct);
                if (courseCheck.Data == null) return _responseHandler.NotFound<LiveSessionResponseDto>("New Course not found.");
            }

            if (dto.InstructorId != session.InstructorId)
            {
                var instructorCheck = await _instructorService.InstructorProfileExists(dto.InstructorId);
                if (instructorCheck.Data == false) return _responseHandler.NotFound<LiveSessionResponseDto>("New Instructor not found.");
            }

            _mapper.Map(dto, session);
            _uow.LiveSessions.Update(session);
            await _uow.SaveChangesAsync(ct);

            var result = await GetByIdAsync(id, ct);
            return _responseHandler.Success(result.Data);
        }
    }
}
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
using Microsoft.EntityFrameworkCore;

namespace E_Learning.Service.Services.LiveSessionServices
{
    public class LiveSessionService : ILiveSessionService
    {
        private readonly IUnitOfWork _uow;
        private readonly ResponseHandler _responseHandler;
        private readonly IMapper _mapper;

        public LiveSessionService(IUnitOfWork uow, ResponseHandler responseHandler, IMapper mapper)
        {
            _uow = uow;
            _responseHandler = responseHandler;
            _mapper = mapper;
        }

        public async Task<Response<string>> CreateAsync(CreateLiveSessionDto dto, CancellationToken ct = default)
        {
            var session = _mapper.Map<LiveSession>(dto);

            await _uow.LiveSessions.AddAsync(session, ct);
            await _uow.SaveChangesAsync(ct);

            return _responseHandler.Created("Live Session created successfully.");
        }

        public async Task<Response<string>> DeleteAsync(int id, CancellationToken ct = default)
        {
            var session = await _uow.LiveSessions.GetByIdAsync(id, ct);

            if (session is null)
                return _responseHandler.NotFound<string>($"Live Session with ID {id} was not found.");

            _uow.LiveSessions.SoftDelete(session);
            await _uow.SaveChangesAsync(ct);

            return _responseHandler.Deleted<string>();
        }

        public async Task<Response<IReadOnlyList<LiveSessionResponseDto>>> GetAllAsync(string? search, int? status, CancellationToken ct = default)
        {
            var query = _uow.LiveSessions.GetTableNoTracking()
                   .Include(x => x.Course)
                   .Include(x => x.Instructor)
                   .Include(x => x.Attendees)
                   .AsQueryable();

            // search
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x => x.Title.Contains(search));
            }

            // fillter
            if (status.HasValue)
            {
                var statusEnum = (LiveSessionStatus)status.Value;
                query = query.Where(x => x.Status == statusEnum);
            }

            // get data
            var sessions = await query.ToListAsync(ct);

            // convert to DTO
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

            return _responseHandler.Success(_mapper.Map<LiveSessionResponseDto>(session));
        }
        

        public async Task<Response<string>> UpdateAsync(int id, UpdateLiveSessionDto dto, CancellationToken ct = default)
        {
            var session = await _uow.LiveSessions.GetByIdAsync(id, ct);

            if (session is null)
                return _responseHandler.NotFound<string>($"Live Session with ID {id} was not found.");

            _mapper.Map(dto, session);

            _uow.LiveSessions.Update(session);
            await _uow.SaveChangesAsync(ct);

            return _responseHandler.Success("Live Session updated successfully.");
        }
    }
}
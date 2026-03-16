using E_Learning.Core.Entities.LiveSessions;
using E_Learning.Core.Entities.Profiles;
using E_Learning.Core.Interfaces.Repositories.LiveSessions;
using E_Learning.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace E_Learning.Repository.Repositories.GenericesRepositories.LiveSessions
{
    public class LiveSessionAttendeeRepository : ILiveSessionAttendeeRepository
    {
        private readonly ELearningDbContext _context;

        public LiveSessionAttendeeRepository(ELearningDbContext context)
        {
            _context = context;
        }
        private IQueryable<LiveSessionAttendee> WithFullIncludes()
        {
               return _context.LiveSessionAttendees
                .Include(a => a.Student) 
                .AsNoTracking();
        }

        public async Task AddAsync(LiveSessionAttendee attendee, CancellationToken ct = default)
        {
            await _context.LiveSessionAttendees.AddAsync(attendee, ct);
        }

        public async Task<IReadOnlyList<LiveSessionAttendee>> GetAttendeesBySessionIdAsync(int sessionId, CancellationToken ct = default)
        {
           return await WithFullIncludes()
                .Where(x => x.SessionId == sessionId)
                .ToListAsync(ct);
        }

        public async Task<bool> IsStudentEnrolledAsync(int sessionId, Guid studentId, CancellationToken ct = default)
        {
           return await _context.LiveSessionAttendees
                .AnyAsync(x => x.SessionId == sessionId && x.StudentId == studentId, ct);
                    }

        public IQueryable<StudentProfile> GetTableNoTracking()
        {
            return _context.StudentProfiles.AsNoTracking();
        
        }
    }
}
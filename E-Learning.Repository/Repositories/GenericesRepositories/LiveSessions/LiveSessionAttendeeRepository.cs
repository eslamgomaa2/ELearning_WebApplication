using E_Learning.Core.Entities.LiveSessions;
using E_Learning.Core.Interfaces.Repositories.LiveSessions;
using E_Learning.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace E_Learning.Repository.Repositories.GenericesRepositories.LiveSessions
{
    public class LiveSessionAttendeeRepository : ILiveSessionAttendeeRepository
    {
        public LiveSessionAttendeeRepository(ELearningDbContext context)
        {
            _context = context;
        }

        public ELearningDbContext _context { get; }

        public async Task AddAsync(LiveSessionAttendee attendee, CancellationToken ct = default)
        {
            await _context.Set<LiveSessionAttendee>().AddAsync(attendee, ct);
        }

        public async Task<IReadOnlyList<LiveSessionAttendee>> GetAttendeesBySessionIdAsync(int sessionId, CancellationToken ct = default)
        {
            return await _context.Set<LiveSessionAttendee>()
                .Where(x => x.SessionId == sessionId)
                .ToListAsync(ct);
        }

        public async Task<bool> IsStudentEnrolledAsync(int sessionId, Guid studentId, CancellationToken ct = default)
        {
            return await _context.Set<LiveSessionAttendee>()
                           .AnyAsync(x => x.SessionId == sessionId && x.StudentId == studentId, ct);
        }
    }
}

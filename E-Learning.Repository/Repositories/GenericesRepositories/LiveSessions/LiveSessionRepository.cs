using E_Learning.Core.Entities.LiveSessions;
using E_Learning.Core.Interfaces.Repositories.LiveSessions;
using E_Learning.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace E_Learning.Repository.Repositories.GenericesRepositories.LiveSessions
{
    public class LiveSessionRepository : ILiveSessionRepository
    {
        public LiveSessionRepository(ELearningDbContext context)
        {
            _context = context;
        }

        public ELearningDbContext _context { get; }

        public async Task AddAsync(LiveSession liveSession, CancellationToken ct = default)
        {
            await _context.LiveSessions.AddAsync(liveSession, ct);
        }

        private IQueryable<LiveSession> WithFullIncludes()
        {
            return _context.LiveSessions
                .Include(ls => ls.Instructor)
                .Include(ls => ls.Course)
                .Include(ls => ls.Attendees);
        }

        public async Task<LiveSession?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await WithFullIncludes()
                .FirstOrDefaultAsync(e => e.Id == id, ct);
        }

        public async Task<IReadOnlyList<LiveSession>> GetAllAsync(CancellationToken ct = default)
        {
            return await WithFullIncludes()
                .ToListAsync(ct);
        }

        public IQueryable<LiveSession> GetTableNoTracking()
        {
            return _context.LiveSessions.AsNoTracking();
        }

        public void SoftDelete(LiveSession liveSession)
        {
            _context.Set<LiveSession>().Remove(liveSession);
        }

        public void Update(LiveSession liveSession)
        {
            _context.LiveSessions.Update(liveSession);
        }


    }
}

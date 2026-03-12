using E_Learning.Core.Entities.LiveSessions;
using E_Learning.Core.Interfaces.Repositories.LiveSessions;
using E_Learning.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace E_Learning.Repository.Repositories.GenericesRepositories.LiveSessions
{
    public class LiveSessionRepository  : ILiveSessionRepository
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

        public async Task<IReadOnlyList<LiveSession>> GetAllAsync(CancellationToken ct = default)
        {
            return await _context.Set<LiveSession>().ToListAsync(ct);
        }

        public async Task<LiveSession?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await _context.LiveSessions
        .FirstOrDefaultAsync(e => e.Id== id ,ct);
        }

        public IQueryable<LiveSession> GetTableNoTracking()
        {
            throw new NotImplementedException();
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

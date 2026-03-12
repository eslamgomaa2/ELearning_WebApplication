using E_Learning.Core.Entities.LiveSessions;

namespace E_Learning.Core.Interfaces.Repositories.LiveSessions
{
    public interface ILiveSessionRepository
    {
        IQueryable<LiveSession> GetTableNoTracking();
        Task<IReadOnlyList<LiveSession>> GetAllAsync(CancellationToken ct = default);
        Task AddAsync(LiveSession liveSession, CancellationToken ct = default);
        void Update(LiveSession liveSession);
        void SoftDelete(LiveSession liveSession);
        Task<LiveSession> GetByIdAsync(int id,CancellationToken ct = default);


    }
}
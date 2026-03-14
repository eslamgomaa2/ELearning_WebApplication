using E_Learning.Core.Entities.Academic;

namespace E_Learning.Core.Interfaces.Repositories.Academic
{
    public interface ILevelRepository
    {
        Task<Level?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<IReadOnlyList<Level>> GetAllAsync(CancellationToken ct = default);
        Task<IReadOnlyList<Level>> GetByStageIdAsync(int stageId, CancellationToken ct = default);
        Task<bool> ExistsAsync(int id, CancellationToken ct = default);
        Task<bool> ExistsAsync(string name, int stageId, CancellationToken ct = default);
        Task AddAsync(Level level, CancellationToken ct = default);
        void Update(Level level);
        void Delete(Level level);
    }
}

using E_Learning.Core.Entities.Academic;

namespace E_Learning.Core.Interfaces.Repositories.Academic
{
    public interface IStageRepository
    {
        Task<Stage?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<IReadOnlyList<Stage>> GetAllAsync(CancellationToken ct = default);
        Task<bool> ExistsAsync(string name, CancellationToken ct = default);
        Task<bool> ExistsAsync(int id, CancellationToken ct = default);
        Task AddAsync(Stage stage, CancellationToken ct = default);
        void Update(Stage stage);
        void Delete(Stage stage);
    }
}

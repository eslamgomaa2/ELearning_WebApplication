using E_Learning.Core.Entities.Academic;
using E_Learning.Core.Interfaces.Repositories.Academic;
using E_Learning.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace E_Learning.Repository.Repositories.GenericesRepositories.Academic
{
    public class StageRepository : IStageRepository
    {
        private readonly ELearningDbContext _context;

        public StageRepository(ELearningDbContext context)
        {
            _context = context;
        }

        private IQueryable<Stage> WithFullIncludes()
        {
            return _context.Stages
                .Include(s => s.Levels);
        }


        public async Task<Stage?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await WithFullIncludes()
                .FirstOrDefaultAsync(s => s.Id == id, ct);
        }

        public async Task<IReadOnlyList<Stage>> GetAllAsync(CancellationToken ct = default)
        {
            return await WithFullIncludes()
                .AsNoTracking()
                .ToListAsync(ct);
        }

        public async Task<bool> ExistsAsync(string name, CancellationToken ct = default)
        {
            return await _context.Stages
                .AnyAsync(s => s.Name.ToLower() == name.ToLower(), ct);
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken ct = default)
        {
            return await _context.Stages
                .AnyAsync(s => s.Id == id, ct);
        }


        public async Task AddAsync(Stage stage, CancellationToken ct = default)
        {
            await _context.Stages.AddAsync(stage, ct);
        }

        public void Update(Stage stage)
        {
            _context.Stages.Update(stage);
        }

        public void Delete(Stage stage)
        {
            _context.Stages.Remove(stage);
        }
    }
}
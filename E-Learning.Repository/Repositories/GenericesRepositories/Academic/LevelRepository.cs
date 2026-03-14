using E_Learning.Core.Entities.Academic;
using E_Learning.Core.Interfaces.Repositories.Academic;
using E_Learning.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace E_Learning.Repository.Repositories.GenericesRepositories.Academic
{
    public class LevelRepository : ILevelRepository
    {
        private readonly ELearningDbContext _context;

        public LevelRepository(ELearningDbContext context)
        {
            _context = context;
        }

        private IQueryable<Level> WithFullIncludes()
        {
            return _context.Levels
                .Include(l => l.Stage);
        }


        public async Task<Level?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await WithFullIncludes()
                .FirstOrDefaultAsync(l => l.Id == id, ct);
        }

        public async Task<IReadOnlyList<Level>> GetAllAsync(CancellationToken ct = default)
        {
            return await WithFullIncludes()
                .AsNoTracking()
                .ToListAsync(ct);
        }

        public async Task<IReadOnlyList<Level>> GetByStageIdAsync(int stageId, CancellationToken ct = default)
        {
            return await WithFullIncludes()
                .Where(l => l.StageId == stageId)
                .AsNoTracking()
                .ToListAsync(ct);
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken ct = default)
        {
            return await _context.Levels
                .AnyAsync(l => l.Id == id, ct);
        }

        public async Task<bool> ExistsAsync(string name, int stageId, CancellationToken ct = default)
        {
            return await _context.Levels
                .AnyAsync(l => l.Name.ToLower() == name.ToLower() && l.StageId == stageId, ct);
        }


        public async Task AddAsync(Level level, CancellationToken ct = default)
        {
            await _context.Levels.AddAsync(level, ct);
        }

        public void Update(Level level)
        {
            _context.Levels.Update(level);
        }

        public void Delete(Level level)
        {
            _context.Levels.Remove(level);
        }
    }
}
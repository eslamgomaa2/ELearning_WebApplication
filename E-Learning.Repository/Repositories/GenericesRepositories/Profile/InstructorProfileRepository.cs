using E_Learning.Core.Entities.Profiles;
using E_Learning.Core.Interfaces.Repositories.Profile;
using E_Learning.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace E_Learning.Repository.Repositories.GenericesRepositories.Profile
{
    public class InstructorProfileRepository : GenericRepository<InstructorProfile, Guid>,IInstructorProfileRepository
    {
        private readonly ELearningDbContext _context;
        public InstructorProfileRepository(ELearningDbContext context):base(context)
        {
            _context = context;
        }
        public async Task<IReadOnlyList<InstructorProfile?>> GetAllInstructorsWithUserAsync()
        {
            return await _context.InstructorProfiles
                .Include(i => i.AppUser)
                .ToListAsync();

        }
        public async Task<InstructorProfile?> GetInstructorProfileWithUserByUserIdAsync(Guid userId)
        {
            return await _context.InstructorProfiles
                .Include(i => i.AppUser)
                .FirstOrDefaultAsync(i => i.AppUserId == userId);

        }
        public async Task<InstructorProfile?> AddAsync(InstructorProfile entity)
        {
            await _context.InstructorProfiles.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}

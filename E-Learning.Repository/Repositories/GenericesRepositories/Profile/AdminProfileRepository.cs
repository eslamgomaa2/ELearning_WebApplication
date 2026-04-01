using E_Learning.Core.Entities.Profiles;
using E_Learning.Core.Interfaces.Repositories.Profile;
using E_Learning.Repository.Data;
using Microsoft.EntityFrameworkCore;


namespace E_Learning.Repository.Repositories.GenericesRepositories.Profile
{
    public class AdminProfileRepository
        : GenericRepository<AdminProfile, Guid>,
          IAdminProfileRepository
    {
        private readonly ELearningDbContext _context;
        public AdminProfileRepository(ELearningDbContext context)
            : base(context) 
        {
            _context = context;
        }

        public async Task<AdminProfile?> GetAdminProfileWithUserAsync(Guid id, CancellationToken ct)
        {
            return await _context.AdminProfiles
                .Include(a => a.AppUser)
                .FirstOrDefaultAsync(a => a.AppUserId == id);
        }
        public async Task<AdminProfile?> GetAdminProfileWithUserByUserIdAsync(Guid userId, CancellationToken ct)
        {
            return await _context.AdminProfiles
                .Include(a => a.AppUser)
                .FirstOrDefaultAsync(a => a.AppUserId == userId);
        }
        public async Task<IReadOnlyList<AdminProfile>> GetAllAdminProfilesWithUsersAsync(CancellationToken ct)
        {
            return await _context.AdminProfiles
                .Include(a => a.AppUser)
                .ToListAsync();

        }

      }
}
   
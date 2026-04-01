using E_Learning.Core.Entities.Profiles;
using E_Learning.Core.Interfaces.Repositories.Profile;
using E_Learning.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace E_Learning.Repository.Repositories.GenericesRepositories.Profile
{
      public class StudentProfileRepository
        : GenericRepository<StudentProfile, Guid>,
          IStudentProfileRepository
    {
        private readonly ELearningDbContext _context;
        public StudentProfileRepository(ELearningDbContext context):base(context) 
        {
            _context = context;
        }
        public async Task<StudentProfile?> GetStudentProfileAsync(Guid id)
        {
            return await _context.StudentProfiles.Include(o=>o.AppUser)
                .FirstOrDefaultAsync(s => s.AppUserId == id);
        }
        public async Task<StudentProfile?> GetStudentProfileWithUserByUserIdAsync(Guid userId)
        {
            return await _context.StudentProfiles
                .Include(s => s.AppUser).ThenInclude(o=>o.NotificationSetting)
                .FirstOrDefaultAsync(s => s.AppUserId == userId);
        }
       


    }
}

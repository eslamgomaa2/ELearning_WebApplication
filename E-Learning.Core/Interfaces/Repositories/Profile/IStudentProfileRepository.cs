using E_Learning.Core.Entities.Profiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Core.Interfaces.Repositories.Profile
{
    public interface IStudentProfileRepository:IGenericRepository<StudentProfile, Guid> 

    {
        public Task<StudentProfile?> GetStudentProfileWithUserAsync(Guid id);
        public Task<StudentProfile?> GetStudentProfileWithUserByUserIdAsync(Guid userId);
        public Task<IReadOnlyList<StudentProfile>> GetAllStudentProfilesWithUsersAsync();
    }
}

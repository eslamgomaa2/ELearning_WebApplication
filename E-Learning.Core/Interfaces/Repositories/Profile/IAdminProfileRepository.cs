using E_Learning.Core.Entities.Profiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Core.Interfaces.Repositories.Profile
{
    public interface IAdminProfileRepository:IGenericRepository<AdminProfile,Guid>

    {
        
        public Task<AdminProfile?> GetAdminProfileWithUserAsync(Guid id);
        public Task<AdminProfile?> GetAdminProfileWithUserByUserIdAsync(Guid userId);
        public Task<IReadOnlyList<AdminProfile>> GetAllAdminProfilesWithUsersAsync();
        


    }
}

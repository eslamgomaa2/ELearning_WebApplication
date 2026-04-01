using E_Learning.Core.Entities.Notifications;
using E_Learning.Core.Entities.Profiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Core.Interfaces.Repositories.Profile
{
    public interface IInstructorProfileRepository:IGenericRepository<InstructorProfile,Guid>
    {
        Task<InstructorProfile?> GetProfileByUserIdAsync(Guid userId);
      


    }
}

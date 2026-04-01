using E_learning.Core.Entities.Identity;
using E_Learning.Core.Entities.Academic;
using E_Learning.Core.Entities.Notifications;
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

        // ── Admin Profile ─────────────────────────────────────────────────────
        Task<AdminProfile?> GetAdminProfileWithUserAsync(Guid userId, CancellationToken ct);
        Task<AdminProfile?> GetAdminProfileWithUserByUserIdAsync(Guid userId,CancellationToken ct);

        

    }
}

using E_learning.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Core.Interfaces.Repositories
{
    public interface IAppUserRepository:IGenericRepository<ApplicationUser,Guid>
    {
    }
}

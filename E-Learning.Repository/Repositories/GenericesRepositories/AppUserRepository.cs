using E_learning.Core.Entities.Identity;
using E_Learning.Core.Interfaces.Repositories;
using E_Learning.Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Repository.Repositories.GenericesRepositories
{
    public class AppUserRepository:GenericRepository<ApplicationUser,Guid>,IAppUserRepository
    {
        public ELearningDbContext _context { get; }

        public AppUserRepository(ELearningDbContext context) : base(context)
        {
            _context = context;
        }
    }
}

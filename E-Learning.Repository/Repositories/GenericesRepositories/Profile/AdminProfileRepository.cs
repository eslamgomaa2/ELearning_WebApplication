using E_Learning.Repository.Data;
using E_Learning.Service.Interfaces.Repositories.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Repository.Repositories.GenericesRepositories.Profile
{
    public class AdminProfileRepository :IAdminProfileRepository
    {
        public ELearningDbContext _context { get; }
        public AdminProfileRepository(ELearningDbContext context)
        {
            _context = context;
        }
    }
}

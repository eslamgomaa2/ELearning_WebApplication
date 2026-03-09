using E_Learning.Repository.Data;
using E_Learning.Service.Interfaces.Repositories.Academic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Repository.Repositories.GenericesRepositories.Academic
{
    public class LevelRepository :ILevelRepository
    {
        public LevelRepository(ELearningDbContext context)
        {
            _context = context;
        }

        public ELearningDbContext _context { get; }
    }
}

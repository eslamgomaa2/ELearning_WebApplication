using E_Learning.Core.Entities.Academic;
using E_Learning.Core.Interfaces.Repositories.Academic;
using E_Learning.Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Repository.Repositories.GenericesRepositories.Academic
{
    public class GradeRangRepository:GenericRepository<GradeRange,int>,IGradeRangRepository
    {
        private readonly ELearningDbContext _context;

        public GradeRangRepository(ELearningDbContext context):base(context)
        {
            _context = context;
        }
    }
}

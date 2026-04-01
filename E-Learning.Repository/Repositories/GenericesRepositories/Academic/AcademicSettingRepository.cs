using E_Learning.Core.Base;
using E_Learning.Core.Entities.Academic;
using E_Learning.Core.Interfaces.Repositories.Academic;
using E_Learning.Core.Repository;
using E_Learning.Repository.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Repository.Repositories.GenericesRepositories.Academic
{
    public class AcademicSettingRepository : GenericRepository<AcademicSetting, int>, IAcademicSettingRepository
    {
        
        private readonly ELearningDbContext _context;

        public AcademicSettingRepository(ELearningDbContext context):base(context)
        {
            _context = context;
        }

        public async Task<AcademicSetting?> GetAcademicSettingAsync()
        {
         return await _context.AcademicSettings.FirstOrDefaultAsync();
        }

        public async  Task<AcademicSetting?> GetAcademicSettingwithGradesAsync(CancellationToken ct)
        {
            return await _context.AcademicSettings
                 .Include(o => o.GradeRanges)
                .FirstOrDefaultAsync();
        }

        }
}

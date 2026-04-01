using E_Learning.Core.Base;
using E_Learning.Core.Entities.Academic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Core.Interfaces.Repositories.Academic
{
    public interface IAcademicSettingRepository:IGenericRepository<AcademicSetting,int>
    {
        public  Task<AcademicSetting?> GetAcademicSettingwithGradesAsync(CancellationToken ct);
        Task<AcademicSetting?> GetAcademicSettingAsync();

    }
}

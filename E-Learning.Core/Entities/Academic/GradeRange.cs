using E_Learning.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Core.Entities.Academic
{
    public class GradeRange:BaseEntity
    {
        public int AcademicSettingId { get; set; }
        public AcademicSetting AcademicSetting { get; set; } = null!;

        public string Letter { get; set; } = string.Empty;  // "A", "B", "C" …
        public int MinScore { get; set; }                   // 90
        public int MaxScore { get; set; }                   // 100
    }
}


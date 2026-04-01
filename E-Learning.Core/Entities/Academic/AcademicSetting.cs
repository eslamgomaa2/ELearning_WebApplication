using E_Learning.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Core.Entities.Academic
{
    public class AcademicSetting:BaseEntity
    {
        // ─── Course Management ────────────────────────────────────────────────
        public bool AllowInstructorsToCreateCourses { get; set; } = true;
        public int MaxCourseDurationWeeks { get; set; } = 10;
        public int MinCourseDurationWeeks { get; set; } = 3;

        // ─── Result Publishing Rules ──────────────────────────────────────────
        public bool AutoPublishResults { get; set; } = false;
        public int ResultReleaseDelayDays { get; set; } = 7;
        public int GradeAppealPeriodDays { get; set; } = 30;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // ─── Navigation ───────────────────────────────────────────────────────
        public ICollection<GradeRange> GradeRanges { get; set; } = new List<GradeRange>();
    }
}

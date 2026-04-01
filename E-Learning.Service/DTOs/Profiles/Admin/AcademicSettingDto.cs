using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Service.DTOs.Profiles.Admin
{
    public class AcademicSettingDto
    {
        // Course Management
        public bool AllowInstructorsToCreateCourses { get; set; }
        public int MaxCourseDurationWeeks { get; set; }
        public int MinCourseDurationWeeks { get; set; }

        // Grading System
        public List<GradeRangeDto> GradeRanges { get; set; } = new();

        // Result Publishing Rules
        public bool AutoPublishResults { get; set; }
        public int ResultReleaseDelayDays { get; set; }
        public int GradeAppealPeriodDays { get; set; }
    }
}

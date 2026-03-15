using E_Learning.Core.Base;
using E_Learning.Core.Entities.Courses;
using E_Learning.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Core.Entities.Enrollment
{
    public class LessonProgress : BaseEntity
    {
        
        public int EnrollmentId { get; set; }
        public Enrollment Enrollment { get; set; } = null!;

        public int LessonId { get; set; }
        public Lesson Lesson { get; set; } = null!;

        public LessonProgressStatus Status { get; set; }
            = LessonProgressStatus.NotStarted;
        public int WatchedSeconds { get; set; } = 0;
        public DateTime? CompletedAt { get; set; }
        public DateTime LastAccessedAt { get; set; } = DateTime.UtcNow;
    }
}

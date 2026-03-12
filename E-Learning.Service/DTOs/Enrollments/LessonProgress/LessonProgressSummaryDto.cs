using E_Learning.Core.Enums;

namespace E_Learning.Service.DTOs.Enrollments.LessonProgress
{
    public class LessonProgressSummaryDto
    {
        public int Id { get; set; }
        public int LessonId { get; set; }
        public LessonSummaryDto Lesson { get; set; } = null!;
        public LessonProgressStatus Status { get; set; }
        public int WatchedSeconds { get; set; }
        public DateTime? CompletedAt { get; set; }
        public DateTime LastAccessedAt { get; set; }
    }
}

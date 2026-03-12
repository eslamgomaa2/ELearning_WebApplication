using E_Learning.Core.Enums;

namespace E_Learning.Service.DTOs.Enrollments.LessonProgress
{
    public class UpdateLessonProgressDto
    {
        public LessonProgressStatus? Status { get; set; }

        public int? WatchedSeconds { get; set; }

        public DateTime? CompletedAt { get; set; }
    }
}

using E_Learning.Core.Enums;
using E_Learning.Service.DTOs.Enrollments.LessonProgress;

namespace E_Learning.Service.DTOs.Enrollments.Enrollment
{
    public class EnrollmentResponseDto
    {
        public int Id { get; set; }
        public Guid StudentId { get; set; }
        public StudentSummaryDto Student { get; set; } = null!;
        public int CourseId { get; set; }
        public CourseSummaryDto Course { get; set; } = null!;
        public int? TransactionId { get; set; }
        public TransactionSummaryDto? Transaction { get; set; }
        public EnrollmentStatus Status { get; set; }
        public decimal ProgressPercentage { get; set; }
        public DateTime EnrolledAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public bool IsDeleted { get; set; }
        public IReadOnlyList<LessonProgressSummaryDto> LessonProgresses { get; set; }
            = new List<LessonProgressSummaryDto>();
    }
}

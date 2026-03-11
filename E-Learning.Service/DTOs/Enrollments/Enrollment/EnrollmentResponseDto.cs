using E_Learning.Core.Enums;

namespace E_Learning.Service.DTOs.Enrollments.Enrollment
{
    public class EnrollmentResponseDto
    {
        public int Id { get; set; }
        public Guid StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public int CourseId { get; set; }
        public string CourseTitle { get; set; } = string.Empty;
        public int? TransactionId { get; set; }
        public EnrollmentStatus Status { get; set; }
        public decimal ProgressPercentage { get; set; }
        public DateTime EnrolledAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}

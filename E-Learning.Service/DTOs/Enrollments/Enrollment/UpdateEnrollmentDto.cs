using E_Learning.Core.Enums;

namespace E_Learning.Service.DTOs.Enrollments.Enrollment
{
    public class UpdateEnrollmentDto
    {
        public EnrollmentStatus? Status { get; set; }

        public decimal? ProgressPercentage { get; set; }

        public DateTime? CompletedAt { get; set; }
    }

}

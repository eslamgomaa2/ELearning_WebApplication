using E_learning.Core.Entities.Identity;
using E_Learning.Core.Base;
using E_Learning.Core.Entities.Courses;
using E_Learning.Core.Enums;
using E_Learning.Core.Entities.Billing;


namespace E_Learning.Core.Entities.Enrollment
{
    public class Enrollment : AuditableEntity, ISoftDelete
    {
        // FK → AppUser
        public Guid StudentId { get; set; }
        public ApplicationUser Student { get; set; } = null!;

        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;

        public int? TransactionId { get; set; }
        public PaymentTransaction? Transaction { get; set; }

        public EnrollmentStatus Status { get; set; }
            = EnrollmentStatus.NotStarted;
        public decimal ProgressPercentage { get; set; } = 0;
        public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;
        public DateTime? CompletedAt { get; set; }

        // ISoftDelete
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }
        
      
    }
}

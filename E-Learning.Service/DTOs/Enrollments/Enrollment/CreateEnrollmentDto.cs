namespace E_Learning.Service.DTOs.Enrollments.Enrollment
{
    public class CreateEnrollmentDto
    {
        public Guid StudentId { get; set; }

        public int CourseId { get; set; }

        public int? TransactionId { get; set; }
    }

}

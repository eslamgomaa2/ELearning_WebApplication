namespace E_Learning.Service.DTOs.Enrollments.LessonProgress
{
    public class CreateLessonProgressDto
    {
        public int EnrollmentId { get; set; }

        public int LessonId { get; set; }

        public int WatchedSeconds { get; set; } = 0;
    }
}

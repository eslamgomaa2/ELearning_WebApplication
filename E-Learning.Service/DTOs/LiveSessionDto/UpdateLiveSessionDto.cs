namespace E_Learning.Service.DTOs.LiveSessionDto
{
    public class UpdateLiveSessionDto
    {
        // نحتاج الـ Id عشان نحدد أي جلسة بدنا نعدل


        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime ScheduledAt { get; set; }
        public int DurationMinutes { get; set; }
        public string RoomName { get; set; } = string.Empty;

        public int CourseId { get; set; }
        public Guid InstructorId { get; set; }

        public bool IsVisibleToStudents { get; set; }
        public int StageId { get; set; }


        public string? Status { get; set; }

        public string? RecordingUrl { get; set; }
    }
}
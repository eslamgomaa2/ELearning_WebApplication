namespace E_Learning.Service.DTOs.Enrollments.Enrollment
{
    public class StudentSummaryDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Bio { get; set; }
        public string? ProfilePicture { get; set; }
        public string? Location { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public bool IsActive { get; set; }
        public DateTime MemberSince { get; set; }
        public string Language { get; set; } = string.Empty;
        public string TimeZone { get; set; } = string.Empty;
        public bool ProfileVisibility { get; set; }
        public bool ShowProgressToOthers { get; set; }
    }

}

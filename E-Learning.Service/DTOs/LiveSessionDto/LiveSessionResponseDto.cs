using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using E_Learning.Service.DTOs.Enrollments.Enrollment;
using E_Learning.Service.DTOs.Profiles.Instructor;

namespace E_Learning.Service.DTOs.LiveSessionDto
{
    public class LiveSessionResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string CourseTitle { get; set; } = string.Empty;
        public Guid InstructorId { get; set; }
        public InstructorResponseDto Instructor { get; set; } = null!;
        public int CourseId { get; set; }
        public CourseSummaryDto Course { get; set; } = null!;
        public DateTime ScheduledAt { get; set; }
        public int DurationMinutes { get; set; }
        public string Status { get; set; }
        public string RoomName { get; set; } = string.Empty;
    }
}
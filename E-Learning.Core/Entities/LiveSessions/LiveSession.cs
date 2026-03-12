using E_learning.Core.Entities.Identity;
using E_Learning.Core.Base;
using E_Learning.Core.Entities.Courses;
using E_Learning.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Core.Entities.LiveSessions
{
    public class LiveSession : AuditableEntity
    {
        public Guid InstructorId { get; set; }
        public ApplicationUser Instructor { get; set; } = null!;

        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;

        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime ScheduledAt { get; set; }
        public int DurationMinutes { get; set; } = 60;
        public string RoomName { get; set; } = string.Empty;
        public string? RecordingUrl { get; set; }
        public string? EgressId { get; set; }
        public bool IsRecorded { get; set; } = true;
        public bool IsVisibleToStudents { get; set; } = false;
        public LiveSessionStatus Status { get; set; }
            = LiveSessionStatus.Scheduled;

        public ICollection<LiveSessionAttendee> Attendees { get; set; }
           = new List<LiveSessionAttendee>();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using E_Learning.Service.DTOs.Enrollments.Enrollment;
using E_Learning.Service.DTOs.Profiles.Student;

namespace E_Learning.Service.DTOs.LiveSessionDto
{
   
        public class AttendeeResponseDto
        {
            public int SessionId { get; set; }
            public LiveSessionResponseDto LiveSession { get; set; } = null!;

            public Guid StudentId { get; set; }
            public StudentSummaryDto Student { get; set; } = null!;

            public DateTime JoinedAt { get; set; }
            public DateTime? LeftAt { get; set; }
            public int? DurationSeconds { get; set; }
        }
    
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using E_Learning.Service.DTOs.Profiles.Student;

namespace E_Learning.Service.DTOs.LiveSessionDto
{
    namespace E_Learning.Service.DTOs.LiveSessionDto
    {
        public class AttendeeResponseDto
        {
            public int SessionId { get; set; }
            public StudentProfileResponseDto Student { get; set; }
            public DateTime JoinedAt { get; set; }
            public DateTime? LeftAt { get; set; }
            public int? DurationSeconds { get; set; }
        }
    }
}
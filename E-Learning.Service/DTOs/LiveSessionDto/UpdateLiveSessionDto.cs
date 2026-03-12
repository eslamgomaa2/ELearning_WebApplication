using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Learning.Service.DTOs.LiveSessionDto
{
    public class UpdateLiveSessionDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime ScheduledAt { get; set; }
        public int DurationMinutes { get; set; }
        public string RoomName { get; set; } = string.Empty;
        
        public bool IsVisibleToStudents { get; set; }
        
        public string? Status { get; set; } 
        
        public string? RecordingUrl { get; set; }
    }
}
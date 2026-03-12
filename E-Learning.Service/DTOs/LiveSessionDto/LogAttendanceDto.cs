using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Learning.Service.DTOs.LiveSessionDto
{
    public class LogAttendanceDto
    {
        public int SessionId { get; set; }
        public Guid StudentId { get; set; }
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Service.DTOs.Profiles.Student
{
    public class StudentProfileResponseDto
    {
        public int profileId { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string? Location { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string? ProfilePicture { get; set; } 
        public DateTime MemberSince { get; set; }
        public decimal EngagementRate { get; set; } 
        public int? LevelId { get; set; }
        
    }
}

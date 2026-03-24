using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Service.DTOs.Profiles.Instructor
{
    public class InstructorResponseDto
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

    }
}

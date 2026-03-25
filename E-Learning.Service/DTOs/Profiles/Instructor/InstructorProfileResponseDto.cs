using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Service.DTOs.Profiles.Instructor
{
    public class InstructorProfileResponseDto
    {
        public int profileId { get; set; }
        public Guid userId { get; set; }

        public string FullName { get; set; }
        public string Email { get; set; }
        public string phoneNumber { get; set; }
        public string Bio { get; set; }
        public string? ProfilePicture { get; set; }

        //public string Password { get; set; }

    }
}

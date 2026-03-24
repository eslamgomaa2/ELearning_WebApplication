using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Service.DTOs.Profiles.Admin
{
    public class AdminProfileResponseDto
    {
        public int Id { get; set; }
        public Guid AppUserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string password { get; set; }
        public string Role { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ProfilePicture { get; set; }
    }
}

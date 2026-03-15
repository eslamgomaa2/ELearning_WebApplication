using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Service.DTOs.Profiles.Admin
{
    public class UserResponseDto
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public string PhoneNumber { get; set; }
        public string Role { get; set; }
       
 

    }
}

using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Service.DTOs.Profiles.Admin
{
    public class CreateAdminProfileDTo
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string phoneNumber { get; set; }
        public bool IsSuperAdmin { get; set; } = false;
        public IFormFile? ProfilePicture { get; set; }
    }
}

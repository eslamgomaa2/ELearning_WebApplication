using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Service.DTOs.Profiles.Admin
{
    public class UpdateAdminProfileDto
    {
        public string? FullName { get; set; }

        public string? PhoneNumber { get; set; }
     
        public IFormFile? ProfilePicture { get; set; }
    }
}

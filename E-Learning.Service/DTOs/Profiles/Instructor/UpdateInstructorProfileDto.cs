using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Service.DTOs.Profiles.Instructor
{
    public class UpdateInstructorProfileDto
    {
        [Required]
        public string ?FullName { get; set; }
        
        public string? phoneNumber { get; set; }
        public string ?Email { get; set; }
        public string ?Bio { get; set; }
        
       
    }
}

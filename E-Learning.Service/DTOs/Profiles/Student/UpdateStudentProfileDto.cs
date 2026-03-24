using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Service.DTOs.Profiles.Student
{
    public class UpdateStudentProfileDto
    {

        public string FullName { get; set; }
        //public string Email { get; set; }
        //public string Password { get; set; }
        public string phoneNumber { get; set; }
        public string location { get; set; }
        public DateOnly? DateOfBirth { get; set; } = null;
        //public DateTime? MemberSince { get; set; } = null;
        public IFormFile? ProfilePicture { get; set; }






    }
}

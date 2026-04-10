using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace E_Learning.Service.DTOs.Profiles.Student
{
    public class UpdateStudentProfileDto
    {
        
        public string? FullName { get; set; }
        
        public string ?phoneNumber { get; set; }
        
        public DateOnly? DateOfBirth { get; set; } = null;
        public IFormFile? ProfilePicture { get; set; }






    }
}

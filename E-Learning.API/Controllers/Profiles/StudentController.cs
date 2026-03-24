using E_Learning.Service.DTOs.Profiles;
using E_Learning.Service.DTOs.Profiles.Admin;
using E_Learning.Service.DTOs.Profiles.Instructor;
using E_Learning.Service.DTOs.Profiles.Student;
using E_Learning.Service.Services.Profiles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace E_Learning.API.Controllers.Profiles
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        /* [HttpPost("create")]
         public async Task<IActionResult> CreateInstructorProfile([FromForm] UpdateStudentProfileDto dto)
         {
             var response = await _studentService.CreateStudentProfile(dto);
             return StatusCode((int)response.HttpStatusCode, response);
         }*/
        [Authorize(Roles = "Student")]
        //[HttpPut("{userId}")]
        [HttpPut("me")]
        public async Task<IActionResult> UpdateStudentProfile( [FromForm] UpdateStudentProfileDto dto)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
                return Unauthorized("User not logged in");

   
            if (!Guid.TryParse(userIdString, out var userId))
                return BadRequest("Invalid user ID");
            var response = await _studentService.UpdateStudentProfile(userId, dto);
            return StatusCode((int)response.HttpStatusCode, response);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetStudentProfileByUserId(Guid userId)
        {
            var response = await _studentService.GetStudentProfileByUserId(userId);
            return StatusCode((int)response.HttpStatusCode, response);
        }

        /*[HttpGet("exists/{userId}")]
        public async Task<IActionResult> StudentProfileExists(Guid userId)
        {
            var response = await _studentService.StudentProfileExists(userId);
            return StatusCode((int)response.HttpStatusCode, response);
        }*/

        [HttpGet("all")]
        public async Task<IActionResult> GetAllStudents()
        {
            var response = await _studentService.GetAllStudents();
            return StatusCode((int)response.HttpStatusCode, response);
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteStudentProfile(Guid userId)
        {
            var response = await _studentService.DeleteStudentProfile(userId);
            return StatusCode((int)response.HttpStatusCode, response);
        }
        [Authorize(Roles = "Student")]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
                return Unauthorized("User not logged in");


            if (!Guid.TryParse(userIdString, out var userId))
                return BadRequest("Invalid user ID");
            var response = await _studentService.ChangePasswordAsync(userId, dto);
            return StatusCode((int)response.HttpStatusCode, response);
        }
    }
}
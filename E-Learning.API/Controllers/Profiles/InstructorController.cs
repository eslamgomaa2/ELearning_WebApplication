using E_Learning.Service.DTOs.Profiles.Instructor;
using E_Learning.Service.Services.Profiles;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace E_Learning.API.Controllers.Profiles
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorController : ControllerBase
    {
        private readonly IInstructorService _instructorService;
        public InstructorController(IInstructorService instructorService)
        {
            _instructorService = instructorService;
        }


        [HttpPost("create")]
        public async Task<IActionResult> CreateInstructorProfile([FromBody] CreateInstructorProfileDto dto)
        {
            var response = await _instructorService.CreateInstructorProfile(dto);
            return StatusCode((int)response.HttpStatusCode, response);
        }
        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateInstructorProfile(Guid userId, [FromBody] CreateInstructorProfileDto dto)
        {
            var response = await _instructorService.UpdateInstructorProfile(userId, dto);
            return StatusCode((int)response.HttpStatusCode, response);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetInstructorProfileByUserId(Guid userId)
        {
            var response = await _instructorService.GetInstructorProfileByUserId(userId);
            return StatusCode((int)response.HttpStatusCode, response);
        }

        [HttpGet("exists/{userId}")]
        public async Task<IActionResult> InstructorProfileExists(Guid userId)
        {
            var response = await _instructorService.InstructorProfileExists(userId);
            return StatusCode((int)response.HttpStatusCode, response);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllInstructors()
        {
            var response = await _instructorService.GetAllInstructors();
            return StatusCode((int)response.HttpStatusCode, response);
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteInstructorProfile(Guid userId)
        {
            var response = await _instructorService.DeleteInstructorProfile(userId);
            return StatusCode((int)response.HttpStatusCode, response);
        }

        [HttpPost("upload-profile-picture")]
        public async Task<IActionResult> UploadProfilePicture(IFormFile file)
        {
            if (file == null)
                return BadRequest("No file uploaded");

            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return BadRequest("User not identified");

            var response = await _instructorService.UploadProfilePicture(Guid.Parse(userIdClaim), file);
            return StatusCode((int)response.HttpStatusCode, response);
        }
    }
}
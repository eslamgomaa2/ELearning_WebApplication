using E_Learning.Service.DTOs.Profiles;
using E_Learning.Service.DTOs.Profiles.Instructor;
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
    public class InstructorController : ControllerBase
    {
        private readonly IInstructorService _instructorService;
        public InstructorController(IInstructorService instructorService)
        {
            _instructorService = instructorService;
        }


        /* [HttpPost("create")]
         public async Task<IActionResult> CreateInstructorProfile([FromBody] CreateInstructorProfileDto dto)
         {
             var response = await _instructorService.CreateInstructorProfile(dto);
             return StatusCode((int)response.HttpStatusCode, response);
         }*/
        //[HttpPut("{userId}")]
        [Authorize(Roles = "Instructor")]

        [HttpPut("me")]
        public async Task<IActionResult> UpdateInstructorProfile( [FromForm] UpdateInstructorProfileDto dto)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
                return Unauthorized("User not logged in");

            if (!Guid.TryParse(userIdString, out var userId))
                return BadRequest("Invalid user ID");
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

        [Authorize(Roles = "Instructor")]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
                return Unauthorized("User not logged in");


            if (!Guid.TryParse(userIdString, out var userId))
                return BadRequest("Invalid user ID");
            var response = await _instructorService.ChangePasswordAsync(userId, dto);
            return StatusCode((int)response.HttpStatusCode, response);
        }
    }
}
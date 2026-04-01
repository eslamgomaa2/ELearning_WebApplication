using E_Learning.Service.DTOs.Profiles;
using E_Learning.Service.DTOs.Profiles.Instructor;
using E_Learning.Service.Services.Profiles.GenericProfileSettingServices;
using E_Learning.Service.Services.Profiles.InstructorSetting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Learning.API.Controllers
{
    [Route("api/settings")]
    [ApiController]
    [Authorize(Roles = "Instructor")]
    public class InstructorSettingsController : ControllerBase
    {
        private readonly IInstructorService _instructorService;
        private readonly IGenericProfileSettingServices _genericProfileSetting;

        public InstructorSettingsController(IInstructorService instructorService, IGenericProfileSettingServices genericProfileSetting)
        {
            _instructorService = instructorService;
            this._genericProfileSetting = genericProfileSetting;
        }

        // ── Hardcoded for testing — replace with JWT claim in production ──────
        private Guid CurrentUserId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        
        [HttpGet]
        public async Task<IActionResult> GetSettings(  CancellationToken ct )
        {
            var result = await _instructorService.GetAllSettingsAsync(CurrentUserId,ct);
            return StatusCode((int)result.HttpStatusCode, result);
        }

        
        [HttpPost("profile-picture")]
        public async Task<IActionResult> UploadProfilePicture(IFormFile file, CancellationToken ct)
        {
            var result = await _genericProfileSetting.UploadProfilePictureAsync(CurrentUserId, file,ct);
            return StatusCode((int)result.HttpStatusCode, result);
        }

        
        [HttpDelete("profile-picture")]
        public async Task<IActionResult> DeleteProfilePicture(CancellationToken ct)
        {
            var result = await _genericProfileSetting.DeleteProfilePictureAsync(CurrentUserId,ct);
            return StatusCode((int)result.HttpStatusCode, result);
        }

        
        [HttpPut("personal-details")]
        public async Task<IActionResult> UpdatePersonalDetails(
            [FromBody] UpdateInstructorProfileDto dto, CancellationToken ct)
        {
            var result = await _instructorService.UpdatePersonalDetailsAsync(CurrentUserId, dto,ct);
            return StatusCode((int)result.HttpStatusCode, result);
        }

        
        [HttpPut("notifications")]
        public async Task<IActionResult> UpdateNotifications(
            [FromBody] InstructorNotificationSettingsDto dto, CancellationToken ct)
        {
            var result = await _instructorService.UpdateInstructorNotificationsAsync(CurrentUserId, dto,ct);
            return StatusCode((int)result.HttpStatusCode, result);
        }

       
        [HttpPut("password")]
        public async Task<IActionResult> UpdatePassword(
            [FromBody] ChangePasswordDto dto)
        {
            var result = await _genericProfileSetting.UpdatePasswordAsync(CurrentUserId, dto);
            return StatusCode((int)result.HttpStatusCode, result);
        }
    }
}

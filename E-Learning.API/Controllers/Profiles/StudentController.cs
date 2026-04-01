using E_Learning.Service.DTOs.Profiles;
using E_Learning.Service.DTOs.Profiles.Student;
using E_Learning.Service.Services.Profiles.GenericProfileSettingServices;
using E_Learning.Service.Services.Profiles.StudentSetting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Learning.API.Controllers
{
    [Route("api/student/settings")]
    [ApiController]
    [Authorize(Roles = "Student")]
    public class StudentSettingsController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly IGenericProfileSettingServices _genericProfileSetting;

        public StudentSettingsController(IStudentService studentService, IGenericProfileSettingServices genericProfileSetting)
        {
            _studentService = studentService;
            _genericProfileSetting = genericProfileSetting;
        }

        // ── Hardcoded for testing — replace with JWT claim ────────────────────
        private Guid CurrentUserId =>  Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        
        [HttpGet]
        public async Task<IActionResult> GetAllSettings()
        {
            var result = await _studentService.GetAllSettingsAsync(CurrentUserId);
            return StatusCode((int)result.HttpStatusCode, result);
        }

        
        [HttpPut("profile")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateProfile(
            [FromForm] UpdateStudentProfileDto dto, CancellationToken ct)
        {
            var result = await _studentService.UpdateStudentInformationAsync(CurrentUserId, dto, ct);
            return StatusCode((int)result.HttpStatusCode, result);
        }

        
        [HttpGet("email-security")]
        public async Task<IActionResult> GetEmailSecurity()
        {
            var result = await _studentService.GetEmailSecurityAsync(CurrentUserId);
            return StatusCode((int)result.HttpStatusCode, result);
        }

  
        [HttpPut("notifications")]
        public async Task<IActionResult> UpdateNotificationsSettings(
            [FromBody] StudentNotificationSettingDto dto,CancellationToken ct)
        {
            var result = await _studentService.UpdateNotificationSettingAsync(CurrentUserId, dto,ct);
            return StatusCode((int)result.HttpStatusCode, result);
        }

        
        [HttpPut("privacy")]
        public async Task<IActionResult> UpdatePrivacy(
            [FromBody] PrivacySettingsDto dto)
        {
            var result = await _studentService.UpdatePrivacySettingsAsync(CurrentUserId, dto);
            return StatusCode((int)result.HttpStatusCode, result);
        }

        [HttpPut("learning-preferences")]
        public async Task<IActionResult> UpdateLearningPreferences(
            [FromBody] LearningPrefrancesDto dto)
        {
            var result = await _studentService.UpdateLearningPrefrancesAsync(CurrentUserId, dto);
            return StatusCode((int)result.HttpStatusCode, result);
        }

        
        [HttpPost("export")]
        public async Task<IActionResult> RequestDataExport()
        {
            var result = await _studentService.RequestDataExportAsync(CurrentUserId);
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

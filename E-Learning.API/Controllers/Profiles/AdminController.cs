using E_Learning.API.Extensions.E_Learning.API.Extensions;
using E_Learning.Service.DTOs.Profiles;
using E_Learning.Service.DTOs.Profiles.Admin;
using E_Learning.Service.Services.Profiles.AdminSetting;
using E_Learning.Service.Services.Profiles.GenericProfileSettingServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles ="Admin")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;
    private readonly IGenericProfileSettingServices _genericProfileSetting;


    public AdminController(IAdminService adminService, IGenericProfileSettingServices genericProfileSetting)
    {
        _adminService = adminService;
        _genericProfileSetting = genericProfileSetting;
    }

    /*private Guid UserId =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);*/


    [HttpGet("settings/notifications")]
    public async Task<IActionResult> GetAdminNotificationSettings(CancellationToken ct)
    {
         
        var UserId = User.GetUserId(); //this extention method get id from jwt token and return it as guid


        var result = await _adminService.GetAdminNotificationSettingsAsync(UserId, ct);
        return StatusCode((int)result.HttpStatusCode, result);
    }

    [HttpGet("profile")]
    public async Task<IActionResult> GetAdminProfile(CancellationToken ct)
    {
       var UserId = User.GetUserId();

        var result = await _adminService.GetAdminProfileByUserId(UserId, ct);
        return StatusCode((int)result.HttpStatusCode, result);
    }

    [HttpPut("profile")]
    public async Task<IActionResult> UpdateAdminProfile([FromForm] UpdateAdminProfileDto dto, CancellationToken ct)
    {
        var UserId = User.GetUserId();
        var result = await _adminService.UpdateAdminProfile(UserId, dto, ct);
        return StatusCode((int)result.HttpStatusCode, result);
    }

    [HttpPut("settings/notifications")]
    public async Task<IActionResult> UpdateAdminNotificationSetting([FromBody] AdminNotificationSettingDto dto, CancellationToken ct)
    {
        var UserId = User.GetUserId();
        var result = await _adminService.UpdateAdminNotificationSettingAsync(UserId, dto, ct);
        return StatusCode((int)result.HttpStatusCode, result);
    }

    [HttpPut("settings/notifications/preferences")]
    public async Task<IActionResult> UpdateAdminNotificationPreferences([FromBody] AdminNotificationPrefrancesDto dto, CancellationToken ct)
    {
        var UserId = User.GetUserId();
        var result = await _adminService.UpdateAdminNotification_PrefrancesSettingAsync(UserId, dto, ct);
        return StatusCode((int)result.HttpStatusCode, result);
    }

    [HttpPut("settings/general")]
    public async Task<IActionResult> UpdateGeneralSetting([FromBody] GeneralSettingDto dto, CancellationToken ct)
    {
        var UserId = User.GetUserId();
        var result = await _adminService.UpdateGeneralSettingAsync(UserId, dto, ct);
        return StatusCode((int)result.HttpStatusCode, result);
    }

    [HttpPut("settings/academic")]
    public async Task<IActionResult> UpdateAcademicSetting([FromBody] AcademicSettingDto dto, CancellationToken ct)
    {
        var UserId = User.GetUserId();
        var result = await _adminService.UpdateAcademicSettingAsync(UserId, dto, ct);
        return StatusCode((int)result.HttpStatusCode, result);
    }

    [HttpPut("password")]
    public async Task<IActionResult> UpdatePassword(
         [FromBody] ChangePasswordDto dto)
    {
        var UserId = User.GetUserId();
        var result = await _genericProfileSetting.UpdatePasswordAsync(UserId, dto);
        return StatusCode((int)result.HttpStatusCode, result);
    }
}
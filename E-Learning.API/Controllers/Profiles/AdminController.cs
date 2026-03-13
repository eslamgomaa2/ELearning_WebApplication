using E_Learning.Core.Base;
using E_Learning.Service.DTOs.Profiles.Admin;
using E_Learning.Service.Services.Profiles;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

[Route("api/[controller]")]
[ApiController]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    // ================= Create Admin Profile =================
    [HttpPost("{userId}")]
    public async Task<IActionResult> CreateAdminProfile(Guid userId, [FromBody] CreateAdminProfileDTo dto)
    {
        var response = await _adminService.CreateAdminProfile(userId, dto);
        return StatusCode((int)response.HttpStatusCode, response);
    }

    // ================= Update Admin Profile =================
    [HttpPut("{userId}")]
    public async Task<IActionResult> UpdateAdminProfile(Guid userId, [FromBody] CreateAdminProfileDTo dto)
    {
        var response = await _adminService.UpdateAdminProfile(userId, dto);
        return StatusCode((int)response.HttpStatusCode, response);
    }

    // ================= Get Admin Profile =================
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetAdminProfileByUserId(Guid userId)
    {
        var response = await _adminService.GetAdminProfileByUserId(userId);
        return StatusCode((int)response.HttpStatusCode, response);
    }

    // ================= Check if Admin Profile Exists =================
    [HttpGet("exists/{userId}")]
    public async Task<IActionResult> AdminProfileExists(Guid userId)
    {
        var response = await _adminService.AdminProfileExists(userId);
        return StatusCode((int)response.HttpStatusCode, response);
    }

    // ================= Get All Admins =================
    [HttpGet("all")]
    public async Task<IActionResult> GetAllAdmins()
    {
        var response = await _adminService.GetAllAdmins();
        return StatusCode((int)response.HttpStatusCode, response);
    }

    // ================= Delete Admin Profile =================
    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteAdminProfile(Guid userId)
    {
        var response = await _adminService.DeleteAdminProfile(userId);
        return StatusCode((int)response.HttpStatusCode, response);
    }

    // ================= Upload Admin Profile Picture =================
    [HttpPost("{userId}/upload-profile-picture")]
    public async Task<IActionResult> UploadProfilePicture(Guid userId, IFormFile file)
    {
        var response = await _adminService.UploadProfilePicture(userId, file);
        return StatusCode((int)response.HttpStatusCode, response);
    }
}
using E_Learning.Core.Base;
using E_Learning.Service.DTOs.Profiles.Admin;
using E_Learning.Service.DTOs.Profiles.Student;
using E_Learning.Service.Services.Profiles;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

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
    [HttpPost("create")]
    public async Task<IActionResult> CreateInstructorProfile([FromBody] CreateAdminProfileDto dto)
    {
        var response = await _adminService.CreateAdminProfile(dto);
        return StatusCode((int)response.HttpStatusCode, response);
    }


    // ================= Update Admin Profile =================
    [HttpPut("update")]
    public async Task<IActionResult> UpdateAdminProfile([FromBody] CreateAdminProfileDto dto)
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
            return BadRequest("User not identified");

        var response = await _adminService.UpdateAdminProfile(Guid.Parse(userIdClaim), dto);
        return StatusCode((int)response.HttpStatusCode, response);
    }

    // ================= Get Admin Profile =================
    [HttpGet("me")]
    public async Task<IActionResult> GetMyProfile()
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
            return BadRequest("User not identified");

        var response = await _adminService.GetAdminProfileByUserId(Guid.Parse(userIdClaim));
        return StatusCode((int)response.HttpStatusCode, response);
    }

    // ================= Check if Admin Profile Exists =================
    [HttpGet("exists")]
    public async Task<IActionResult> AdminProfileExists()
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
            return BadRequest("User not identified");

        var response = await _adminService.AdminProfileExists(Guid.Parse(userIdClaim));
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
    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteMyProfile()
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
            return BadRequest("User not identified");

        var response = await _adminService.DeleteAdminProfile(Guid.Parse(userIdClaim));
        return StatusCode((int)response.HttpStatusCode, response);
    }

    // ================= Upload Admin Profile Picture =================
    [HttpPost("upload-profile-picture")]
    public async Task<IActionResult> UploadProfilePicture(IFormFile file)
    {
        if (file == null)
            return BadRequest("No file uploaded");

        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
            return BadRequest("User not identified");

        var response = await _adminService.UploadProfilePicture(Guid.Parse(userIdClaim), file);
        return StatusCode((int)response.HttpStatusCode, response);
    }
}
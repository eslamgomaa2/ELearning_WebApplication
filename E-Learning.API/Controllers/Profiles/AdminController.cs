using E_Learning.Core.Base;
using E_Learning.Service.DTOs.Profiles.Admin;
using E_Learning.Service.DTOs.Profiles.Instructor;
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
        private readonly IInstructorService _instructorService;

    public AdminController(IAdminService adminService, IInstructorService instructorService)
    {
        _adminService = adminService;
        _instructorService = instructorService;
    }

    // ================= Create Admin Profile =================
    [HttpPost("create")]
    public async Task<IActionResult> CreateAdminProfile([FromForm] CreateAdminProfileDto dto)
    {
        var response = await _adminService.CreateUserProfile(dto);
        return StatusCode((int)response.HttpStatusCode, response);
    }

    /*// ================= Create Instructor Profile =================
    [HttpPost("create-instructor")]
    public async Task<IActionResult> CreateInstructorProfile([FromForm] CreateInstructorProfileDto dto)
    {
        var response = await _adminService.CreateInstructorProfile(dto);
        return StatusCode((int)response.HttpStatusCode, response);
    }*/




    // ================= Update Admin Profile =================
    [HttpPut("{userId}")]
    public async Task<IActionResult> UpdateAdminProfile(Guid userId, [FromForm] UpdateAdminProfileDto dto)
    {
        var response = await _adminService.UpdateAdminProfile(userId, dto);
        return StatusCode((int)response.HttpStatusCode, response);
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetAdminByUserId(Guid userId)
    {
        var response = await _adminService.GetAdminProfileByUserId(userId);
        return StatusCode((int)response.HttpStatusCode, response);
    }

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


}
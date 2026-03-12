// E_Learning.API/Controllers/AuthController.cs
using E_Learning.Core.Interfaces.Services;
using E_Learning.Service.DTOs.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Learning.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _auth;
    public AuthController(IAuthService auth) => _auth = auth;

    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterRequestDto dto,
        CancellationToken ct)
    {
        var msg = await _auth.RegisterAsync(dto, ct);
        return Ok(new { message = msg });
    }

    [HttpPost("verify-email")]
    public async Task<IActionResult> VerifyEmail(
        [FromBody] VerifyEmailRequestDto dto,
        CancellationToken ct)
    {
        var result = await _auth.VerifyEmailAsync(dto, ct);
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequestDto dto,
        CancellationToken ct)
    {
        var result = await _auth.LoginAsync(dto, ct);
        return Ok(result);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(
        [FromBody] RefreshTokenRequestDto dto,
        CancellationToken ct)
    {
        var result = await _auth.RefreshTokenAsync(dto.RefreshToken, ct);
        return Ok(result);
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout(
        [FromBody] RefreshTokenRequestDto dto,
        CancellationToken ct)
    {
        await _auth.LogoutAsync(dto.RefreshToken, ct);
        return Ok(new { message = "Logged out successfully" });
    }

    /// <summary>Send password reset OTP to email</summary>
    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(
        [FromBody] ForgotPasswordRequestDto dto,
        CancellationToken ct)
    {
        await _auth.ForgotPasswordAsync(dto, ct);
        return Ok(new { message = "If this email exists, a reset code has been sent" });
    }

    /// <summary>Reset password using OTP code</summary>
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(
        [FromBody] ResetPasswordRequestDto dto,
        CancellationToken ct)
    {
        await _auth.ResetPasswordAsync(dto, ct);
        return Ok(new { message = "Password reset successfully" });
    }

    /// <summary>Login or register using Google OAuth</summary>
    [HttpPost("google")]
    public async Task<IActionResult> GoogleLogin(
        [FromBody] GoogleLoginRequestDto dto,
        CancellationToken ct)
    {
        var result = await _auth.GoogleLoginAsync(dto, ct);
        return Ok(result);
    }
}
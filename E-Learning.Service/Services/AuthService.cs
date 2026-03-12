using E_learning.Core.Entities.Identity;
using E_Learning.Core.Entities.Identity;
using E_Learning.Core.Exceptions;
using E_Learning.Core.Interfaces.Services;
using E_Learning.Core.Repository;
using E_Learning.Service.Contract;
using E_Learning.Service.DTOs.Auth;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace E_Learning.Service.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUnitOfWork _uow;
    private readonly IConfiguration _config;
    private readonly IEmailService _emailService;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        IUnitOfWork uow,
        IConfiguration config,
        IEmailService emailService)
    {
        _userManager = userManager;
        _uow = uow;
        _config = config;
        _emailService = emailService;
    }

    // ═══════════════════════════════════════════════════════════
    // REGISTER
    // ═══════════════════════════════════════════════════════════
    public async Task<string> RegisterAsync(
        RegisterRequestDto dto,
        CancellationToken ct = default)
    {
        // 1. Check duplicate email
        var existing = await _userManager.FindByEmailAsync(dto.Email);
        if (existing is not null)
            throw new BadRequestException("Email is already registered");

        // 2. Create ApplicationUser
        var user = new ApplicationUser
        {
            FullName = dto.FullName,
            UserName = dto.Email,
            Email = dto.Email,
            MemberSince = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsActive = true,
            Language = "en",
            TimeZone = "UTC"
        };

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
            throw new E_Learning.Core.Exceptions.ValidationException(ToErrorDict(result.Errors));

        // 3. Assign default Student role
        await _userManager.AddToRoleAsync(user, "Student");

        // 4. Generate OTP and send verification email
        var otp = GenerateOtp();
        await SaveOtpAsync(user.Id, otp, "EmailVerification", ct);
        //await _emailService.SendEmailVerificationOtpAsync(user.UserName, otp);
        await _emailService.SendEmailVerificationOtpAsync(user.Email!, otp);

        return "Registration successful. Please check your email for the verification code.";
    }

    // ═══════════════════════════════════════════════════════════
    // VERIFY EMAIL
    // ═══════════════════════════════════════════════════════════
    public async Task<AuthResponseDto> VerifyEmailAsync(
        VerifyEmailRequestDto dto,
        CancellationToken ct = default)
    {
        // 1. Find user
        var user = await _userManager.FindByEmailAsync(dto.Email)
            ?? throw new BadRequestException("Invalid request");

        // 2. Check not already verified
        if (user.EmailConfirmed)
            throw new BadRequestException("Email is already verified");

        // 3. Validate OTP
        var otp = await _uow.OtpCodes
            .GetValidOtpAsync(user.Id, dto.OtpCode, "EmailVerification", ct)
            ?? throw new BadRequestException("Invalid or expired verification code");

        // 4. Confirm email
        user.EmailConfirmed = true;
        await _userManager.UpdateAsync(user);

        // 5. Mark OTP as used
        otp.IsUsed = true;
        _uow.OtpCodes.Update(otp);
        await _uow.SaveChangesAsync(ct);

        // 6. Return tokens
        return await BuildAuthResponseAsync(user, ct);
    }

    // ═══════════════════════════════════════════════════════════
    // LOGIN
    // ═══════════════════════════════════════════════════════════
    public async Task<AuthResponseDto> LoginAsync(
        LoginRequestDto dto,
        CancellationToken ct = default)
    {
        // 1. Find user
        var user = await _userManager.FindByEmailAsync(dto.Email)
            ?? throw new UnauthorizedException("Invalid email or password");

        // 2. Check email verified
        if (!user.EmailConfirmed)
            throw new ForbiddenException("Please verify your email first");

        // 3. Check account active
        if (!user.IsActive)
            throw new ForbiddenException("Your account has been deactivated");

        // 4. Verify password
        if (!await _userManager.CheckPasswordAsync(user, dto.Password))
            throw new UnauthorizedException("Invalid email or password");

        // 5. Return tokens
        return await BuildAuthResponseAsync(user, ct);
    }

    // ═══════════════════════════════════════════════════════════
    // REFRESH TOKEN
    // ═══════════════════════════════════════════════════════════
    public async Task<AuthResponseDto> RefreshTokenAsync(
        string refreshToken,
        CancellationToken ct = default)
    {
        // 1. Find active session
        var session = await _uow.UserSessions
            .GetActiveSessionByTokenAsync(refreshToken, ct)
            ?? throw new BadRequestException("Invalid or expired refresh token");

        // 2. Find user
        var user = await _userManager.FindByIdAsync(session.UserId.ToString())
            ?? throw new UnauthorizedException("User not found");

        // 3. Revoke old session (Rotate token)
        session.IsActive = false;
        _uow.UserSessions.Update(session);
        await _uow.SaveChangesAsync(ct);

        // 4. Return new tokens
        return await BuildAuthResponseAsync(user, ct);
    }

    // ═══════════════════════════════════════════════════════════
    // LOGOUT
    // ═══════════════════════════════════════════════════════════
    public async Task LogoutAsync(
        string refreshToken,
        CancellationToken ct = default)
    {
        // Find active session
        var session = await _uow.UserSessions
            .GetActiveSessionByTokenAsync(refreshToken, ct);

        // Silent if already invalid
        if (session is null) return;

        // Revoke session
        session.IsActive = false;
        _uow.UserSessions.Update(session);
        await _uow.SaveChangesAsync(ct);
    }

    // ═══════════════════════════════════════════════════════════
    // FORGOT PASSWORD
    // ═══════════════════════════════════════════════════════════
    public async Task ForgotPasswordAsync(
        ForgotPasswordRequestDto dto,
        CancellationToken ct = default)
    {
        // Silent fail — don't reveal if email exists (security)
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user is null) return;

        // Generate OTP and send email
        var otp = GenerateOtp();
        await SaveOtpAsync(user.Id, otp, "PasswordReset", ct);
        await _emailService.SendPasswordResetOtpAsync(user.Email!, otp);
    }

    // ═══════════════════════════════════════════════════════════
    // RESET PASSWORD
    // ═══════════════════════════════════════════════════════════
    public async Task ResetPasswordAsync(
        ResetPasswordRequestDto dto,
        CancellationToken ct = default)
    {
        // 1. Find user
        var user = await _userManager.FindByEmailAsync(dto.Email)
            ?? throw new NotFoundException("User not found");

        // 2. Validate OTP
        var otp = await _uow.OtpCodes
            .GetValidOtpAsync(user.Id, dto.OtpCode, "PasswordReset", ct)
            ?? throw new BadRequestException("Invalid or expired code");

        // 3. Reset password via Identity (generates internal reset token)
        var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, resetToken, dto.NewPassword);

        if (!result.Succeeded)
            throw new Core.Exceptions.ValidationException(ToErrorDict(result.Errors));

        // 4. Mark OTP as used
        otp.IsUsed = true;
        _uow.OtpCodes.Update(otp);

        // 5. Revoke all active sessions (security — force re-login)
        await _uow.UserSessions.RevokeAllUserSessionsAsync(user.Id, ct);

        await _uow.SaveChangesAsync(ct);
    }

    // ═══════════════════════════════════════════════════════════
    // GOOGLE LOGIN
    // ═══════════════════════════════════════════════════════════
    public async Task<AuthResponseDto> GoogleLoginAsync(
        GoogleLoginRequestDto dto,
        CancellationToken ct = default)
    {
        // 1. Verify Google ID token
        GoogleJsonWebSignature.Payload payload;
        try
        {
            payload = await GoogleJsonWebSignature.ValidateAsync(
                dto.IdToken,
                new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new[] { _config["Google:ClientId"] }
                });
        }
        catch
        {
            throw new BadRequestException("Invalid Google token");
        }

        // 2. Find or create user
        var user = await _userManager.FindByEmailAsync(payload.Email);

        if (user is null)
        {
            // First time Google login — create account
            user = new ApplicationUser
            {
                FullName = $"{payload.GivenName} {payload.FamilyName}".Trim(),
                UserName = payload.Email,
                Email = payload.Email,
                EmailConfirmed = true, // Google already verified
            //    ProfilePicture = payload.Picture,
                MemberSince = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true,
                Language = "en",
                TimeZone = "UTC"
              
            };

            var result = await _userManager.CreateAsync(user);
            if (!result.Succeeded)
                throw new Core.Exceptions.ValidationException(ToErrorDict(result.Errors));

            await _userManager.AddToRoleAsync(user, "Student");
        }

        // 3. Return tokens
        return await BuildAuthResponseAsync(user, ct);
    }

    // ═══════════════════════════════════════════════════════════
    // PRIVATE HELPERS
    // ═══════════════════════════════════════════════════════════

    /// <summary>
    /// Shared builder — generates JWT + RefreshToken + saves UserSession
    /// Called by: Login, VerifyEmail, RefreshToken, GoogleLogin
    /// </summary>
    private async Task<AuthResponseDto> BuildAuthResponseAsync(
        ApplicationUser user,
        CancellationToken ct = default)
    {
        var roles = await _userManager.GetRolesAsync(user);
        var accessToken = GenerateJwt(user, roles.ToList());
        var refreshToken = GenerateRefreshToken();

        // Save new session to DB
        var session = new UserSession
        {
            UserId = user.Id,
            RefreshToken = refreshToken,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            LastActivityAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(GetRefreshDays())
        };

        await _uow.UserSessions.AddAsync(session, ct);
        await _uow.SaveChangesAsync(ct);

        return new AuthResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(GetAccessMinutes()),
            User = new UserInfoDto
            {
                Id = user.Id.ToString(),
                FullName = user.FullName,
                Email = user.Email!,
              //  ProfilePicture = user.ProfilePicture,
                Roles = roles.ToList()
            }
        };
    }

    /// <summary>
    /// Generates signed JWT access token with user claims
    /// </summary>
    private string GenerateJwt(ApplicationUser user, List<string> roles)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier,        user.Id.ToString()),
            new(ClaimTypes.Email,                  user.Email!),
            new(ClaimTypes.Name,                   user.FullName),
            new(JwtRegisteredClaimNames.Sub,        user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email,      user.Email!),
            new(JwtRegisteredClaimNames.Jti,        Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat,
                DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString())
        };

        // Add role claims
        foreach (var role in roles)
            claims.Add(new Claim(ClaimTypes.Role, role));

        var key = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(GetAccessMinutes()),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    /// <summary>
    /// Generates cryptographically secure random refresh token
    /// </summary>
    private static string GenerateRefreshToken()
    {
        var bytes = new byte[64];
        RandomNumberGenerator.Fill(bytes);
        return Convert.ToBase64String(bytes);
    }

    /// <summary>
    /// Generates 6-digit OTP code
    /// </summary>
    private static string GenerateOtp()
        => Random.Shared.Next(100000, 999999).ToString();

    /// <summary>
    /// Saves OTP to DB with 5-minute expiry
    /// </summary>
    private async Task SaveOtpAsync(
        Guid userId, string code, string purpose, CancellationToken ct)
    {
        var otp = new OtpCode
        {
            UserId = userId,
            Code = code,
            Purpose = purpose,
            ExpiresAt = DateTime.UtcNow.AddMinutes(5),
            IsUsed = false
        };

        await _uow.OtpCodes.AddAsync(otp, ct);
        await _uow.SaveChangesAsync(ct);
    }

    /// <summary>
    /// Converts Identity errors to Dictionary for ValidationException
    /// </summary>
    private static Dictionary<string, string[]> ToErrorDict(
        IEnumerable<IdentityError> errors)
        => errors
            .GroupBy(e => e.Code)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.Description).ToArray());

    private int GetAccessMinutes()
        => int.TryParse(_config["Jwt:AccessTokenExpiryMinutes"], out var m) ? m : 60;

    private int GetRefreshDays()
        => int.TryParse(_config["Jwt:RefreshTokenExpiryDays"], out var d) ? d : 7;
}

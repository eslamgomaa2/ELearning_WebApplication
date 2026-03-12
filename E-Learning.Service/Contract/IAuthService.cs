using E_Learning.Service.DTOs.Auth;

namespace E_Learning.Core.Interfaces.Services;

public interface IAuthService
{
    Task<string> RegisterAsync(
        RegisterRequestDto dto,
        CancellationToken ct = default);

    Task<AuthResponseDto> VerifyEmailAsync(
        VerifyEmailRequestDto dto,
        CancellationToken ct = default);

    Task<AuthResponseDto> LoginAsync(
        LoginRequestDto dto,
        CancellationToken ct = default);

    Task<AuthResponseDto> RefreshTokenAsync(
        string refreshToken,
        CancellationToken ct = default);

    Task LogoutAsync(
        string refreshToken,
        CancellationToken ct = default);

    Task ForgotPasswordAsync(
        ForgotPasswordRequestDto dto,
        CancellationToken ct = default);

    Task ResetPasswordAsync(
        ResetPasswordRequestDto dto,
        CancellationToken ct = default);

    Task<AuthResponseDto> GoogleLoginAsync(
        GoogleLoginRequestDto dto,
        CancellationToken ct = default);
}
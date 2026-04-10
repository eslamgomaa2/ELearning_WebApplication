namespace E_Learning.Service.Contract;

public interface IEmailService
{
    Task SendEmailVerificationOtpAsync(string toEmail, string otpCode);
    Task SendPasswordResetOtpAsync(string toEmail, string otpCode);
    Task SendInstructorApprovedAsync(string toEmail, string fullName);
    Task SendInstructorRejectedAsync(string toEmail, string fullName);
}
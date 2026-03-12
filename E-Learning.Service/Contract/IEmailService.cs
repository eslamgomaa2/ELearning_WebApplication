namespace E_Learning.Service.Contract;

public interface IEmailService
{
    Task SendEmailVerificationOtpAsync(string toEmail, string otpCode);
    Task SendPasswordResetOtpAsync(string toEmail, string otpCode);
}
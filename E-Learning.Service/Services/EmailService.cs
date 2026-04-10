using E_Learning.Core.Helper;
using E_Learning.Service.Contract;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;



public class EmailService : IEmailService
{
    private readonly MailSetting _mailSettings;

    public EmailService(IOptions<MailSetting> mailSetting)
    {
        _mailSettings = mailSetting.Value;
    }

    public Task SendEmailVerificationOtpAsync(string toEmail, string otpCode) =>
        SendEmailAsync(toEmail, EmailType.VerifyEmailOtp, otpCode);

    public Task SendPasswordResetOtpAsync(string toEmail, string otpCode) =>
        SendEmailAsync(toEmail, EmailType.ResetPasswordOtp, otpCode);

    public Task SendInstructorApprovedAsync(string toEmail, string fullName) =>
        SendEmailAsync(toEmail, EmailType.InstructorApproved, fullName);

    public Task SendInstructorRejectedAsync(string toEmail, string fullName) =>
        SendEmailAsync(toEmail, EmailType.InstructorRejected, fullName);

    private async Task SendEmailAsync(string to, EmailType type, string value)
    {
        var (subject, body) = BuildEmailContent(type, value);
        await SendAsync(to, subject, body);
    }

    private (string Subject, string Body) BuildEmailContent(EmailType type, string value)
    {
        return type switch
        {
            EmailType.VerifyEmailOtp => (
                "Verify Your Email - E-Learning Platform",
                $"""
                <h2>Email Verification</h2>
                <p>Your verification code is:</p>
                <h1 style="color:#4F46E5; letter-spacing:8px;">{value}</h1>
                <p>This code expires in <strong>5 minutes</strong>.</p>
                """
            ),

            EmailType.ResetPasswordOtp => (
                "Reset Your Password - E-Learning Platform",
                $"""
                <h2>Password Reset</h2>
                <p>Your password reset code is:</p>
                <h1 style="color:#DC2626; letter-spacing:8px;">{value}</h1>
                <p>This code expires in <strong>5 minutes</strong>.</p>
                <p>If you didn't request this, please ignore this email.</p>
                """
            ),

            EmailType.InstructorApproved => (
                "Your Instructor Application Has Been Approved!",
                $"""
                <h2>Congratulations, {value}! 🎉</h2>
                <p>Your instructor application has been <strong style="color:#16A34A;">approved</strong>.</p>
                <p>You can now log in and start creating courses.</p>
                """
            ),

            EmailType.InstructorRejected => (
                "Your Instructor Application Status",
                $"""
                <h2>Hello, {value}</h2>
                <p>Unfortunately, your instructor application has been <strong style="color:#DC2626;">rejected</strong>.</p>
                <p>If you believe this is a mistake, please contact our support team.</p>
                """
            ),

            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }

    private async Task SendAsync(string to, string subject, string htmlBody)
    {
        var email = new MimeMessage();
        email.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.FromEmail!));
        email.To.Add(MailboxAddress.Parse(to));
        email.Subject = subject;

        email.Body = new BodyBuilder { HtmlBody = htmlBody }.ToMessageBody();

        using var smtp = new SmtpClient();

        smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;

        await smtp.ConnectAsync(_mailSettings.SmtpHost, 465, SecureSocketOptions.SslOnConnect);
        await smtp.AuthenticateAsync(_mailSettings.FromEmail, _mailSettings.Password);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}
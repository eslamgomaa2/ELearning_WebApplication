using E_Learning.Service.Contract;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace E_Learning.Service.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config) => _config = config;

    public async Task SendEmailVerificationOtpAsync(string toEmail, string otpCode)
    {
        var subject = "Verify Your Email - E-Learning Platform";
        var body = $"""
            <h2>Email Verification</h2>
            <p>Your verification code is:</p>
            <h1 style="color:#4F46E5; letter-spacing:8px;">{otpCode}</h1>
            <p>This code expires in <strong>5 minutes</strong>.</p>
            """;
        await SendAsync(toEmail, subject, body);
    }

    public async Task SendPasswordResetOtpAsync(string toEmail, string otpCode)
    {
        var subject = "Reset Your Password - E-Learning Platform";
        var body = $"""
            <h2>Password Reset</h2>
            <p>Your password reset code is:</p>
            <h1 style="color:#DC2626; letter-spacing:8px;">{otpCode}</h1>
            <p>This code expires in <strong>5 minutes</strong>.</p>
            <p>If you didn't request this, please ignore this email.</p>
            """;
        await SendAsync(toEmail, subject, body);
    }
    private async Task SendAsync(string to, string subject, string htmlBody)
    {
        var smtpHost = _config["Email:SmtpHost"] ?? "sandbox.smtp.mailtrap.io";
        var smtpPort = int.Parse(_config["Email:SmtpPort"] ?? "2525");
        var smtpUser = _config["Email:Username"]
            ?? throw new InvalidOperationException("Email:Username is not configured");
        var smtpPass = _config["Email:Password"]
            ?? throw new InvalidOperationException("Email:Password is not configured");
        var fromName = _config["Email:FromName"] ?? "E-Learning Platform";
        var fromEmail = _config["Email:FromEmail"] ?? "noreply@elearning.com"; 

        using var client = new SmtpClient(smtpHost, smtpPort)
        {
            Credentials = new NetworkCredential(smtpUser, smtpPass),
            EnableSsl = true
        };

        var message = new MailMessage
        {
            From = new MailAddress(fromEmail, fromName), 
            Subject = subject,
            Body = htmlBody,
            IsBodyHtml = true
        };
        message.To.Add(to.Trim());

        await client.SendMailAsync(message);
    }

}
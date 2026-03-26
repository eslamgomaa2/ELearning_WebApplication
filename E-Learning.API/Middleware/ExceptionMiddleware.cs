// E_Learning.API/Middleware/ExceptionMiddleware.cs
using E_Learning.Core.Exceptions;
using System.Text.Json;

namespace E_Learning.API.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";

        var response = ex switch
        {
            // ── 422 Validation ───────────────────────────────
            ValidationException ve => BuildResponse(
                context, 422, "VALIDATION_ERROR",
                "Validation failed",
                ve.Errors),

            // ── 400 Bad Request ──────────────────────────────
            BadRequestException be => BuildResponse(
                context, 400, "BAD_REQUEST",
                be.Message),

            // ── 401 Unauthorized ─────────────────────────────
            UnauthorizedException ue => BuildResponse(
                context, 401, "UNAUTHORIZED",
                ue.Message),

            // ── 403 Forbidden ────────────────────────────────
            ForbiddenException fe => BuildResponse(
                context, 403, "FORBIDDEN",
                fe.Message),

            // ── 404 Not Found ────────────────────────────────
            NotFoundException nfe => BuildResponse(
                context, 404, "NOT_FOUND",
                nfe.Message),

            // ── 409 Conflict ─────────────────────────────────
            ConflictException ce => BuildResponse(
                context, 409, "CONFLICT",
                ce.Message),

            // ── 500 Unexpected ───────────────────────────────
            _ => BuildResponse(
                context, 500, "INTERNAL_SERVER_ERROR",
                "An unexpected error occurred")
        };

        // Log — بس مش للـ 4xx العادية
        if (context.Response.StatusCode >= 500)
            _logger.LogError(ex, "❌ [{StatusCode}] {Message}",
                context.Response.StatusCode, ex.Message);
        else
            _logger.LogWarning("⚠️ [{StatusCode}] {Message}",
                context.Response.StatusCode, ex.Message);

        await context.Response.WriteAsync(JsonSerializer.Serialize(
            response,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }));
    }

    private static object BuildResponse(
        HttpContext context,
        int statusCode,
        string errorCode,
        string message,
        object? errors = null)
    {
        context.Response.StatusCode = statusCode;

        return new
        {
            success = false,
            statusCode,
            errorCode,
            message,
            errors,                              // null لو مش Validation
            path = context.Request.Path.Value,
            timestamp = DateTime.UtcNow
        };
    }
}
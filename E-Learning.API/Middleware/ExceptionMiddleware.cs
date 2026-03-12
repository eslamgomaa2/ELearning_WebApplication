using E_Learning.Core.Exceptions;
using System.Text.Json;

namespace E_Learning.API.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
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
            _logger.LogError(ex, ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";

        object response;

        switch (ex)
        {
            case ValidationException ve:
                context.Response.StatusCode = 422;
                response = new { message = ve.Message, errors = ve.Errors };
                break;

            case AppException ae:
                context.Response.StatusCode = ae.StatusCode;
                response = new { message = ae.Message };
                break;

            default:
                context.Response.StatusCode = 500;
                response = new { message = "An unexpected error occurred" };
                break;
        }

        await context.Response.WriteAsync(JsonSerializer.Serialize(response,
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
    }
}
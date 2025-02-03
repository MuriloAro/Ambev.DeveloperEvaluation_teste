using System.Net;
using System.Text.Json;

namespace Ambev.DeveloperEvaluation.WebApi.Middleware;

public class UnauthorizedMiddleware
{
    private readonly RequestDelegate _next;

    public UnauthorizedMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await _next(context);

        if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
        {
            context.Response.ContentType = "application/json";
            
            var response = new
            {
                success = false,
                message = "Unauthorized access. Please provide a valid authentication token.",
                details = "Token is missing or invalid",
                statusCode = 401
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}

public static class UnauthorizedMiddlewareExtensions
{
    public static IApplicationBuilder UseUnauthorizedHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<UnauthorizedMiddleware>();
    }
} 
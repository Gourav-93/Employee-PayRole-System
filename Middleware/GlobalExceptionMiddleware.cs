using System.Text.Json;

namespace EmployeeManagementPayrollSystem.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";

            var response = new
            {
                statusCode = 500,
                message = "An unexpected error occurred.",
                error = ex.Message
            };

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response));
        }
    }
}
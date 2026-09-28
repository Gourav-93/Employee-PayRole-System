using EmployeeManagementPayrollSystem.Middleware;

namespace EmployeeManagementPayrollSystem;

public static class GlobalExceptionMiddlewareExtensions
{
    public static IApplicationBuilder
        UseGlobalExceptionHandling(
            this IApplicationBuilder app)
    {
        return app.UseMiddleware<
            GlobalExceptionMiddleware>();
    }
}
namespace ServerAspNetCoreAPIMakePC.API.Middleware
{
    using Domain.Enums;

    public class AdminAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AdminAuthorizationMiddleware> _logger;

        public AdminAuthorizationMiddleware(RequestDelegate next, ILogger<AdminAuthorizationMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Check if this is an admin area request
                if (!IsAdminArea(context.Request.Path))
                {
                    await _next(context);
                    return;
                }

                // Check if user is authenticated
                if (!context.User.Identity.IsAuthenticated)
                {
                    _logger.LogWarning("Unauthenticated access attempt to admin area: {Path}", context.Request.Path);
                    await RedirectToLogin(context);
                    return;
                }

                // Get user role from context (set by RoleAuthorizationMiddleware)
                if (!context.Items.TryGetValue("UserRole", out var roleObj) || roleObj is not UserRole userRole)
                {
                    _logger.LogWarning("No user role found in context for admin area access: {Path}", context.Request.Path);
                    await HandleForbidden(context);
                    return;
                }

                // Check if user has admin privileges
                if (!IsAdminRole(userRole))
                {
                    var userId = context.Items["UserId"];
                    _logger.LogWarning("User {UserId} with role {Role} attempted to access admin area: {Path}",
                        userId, userRole, context.Request.Path);
                    await HandleForbidden(context);
                    return;
                }

                // Check specific admin permissions for sensitive operations
                if (IsSensitiveAdminOperation(context.Request.Path, context.Request.Method))
                {
                    if (!HasSensitiveAdminPermissions(userRole))
                    {
                        var userId = context.Items["UserId"];
                        _logger.LogWarning("User {UserId} with role {Role} attempted sensitive admin operation: {Method} {Path}",
                            userId, userRole, context.Request.Method, context.Request.Path);
                        await HandleForbidden(context);
                        return;
                    }
                }

                _logger.LogDebug("Admin access granted to user with role {Role} for {Path}", userRole, context.Request.Path);

                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AdminAuthorizationMiddleware for path {Path}", context.Request.Path);
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync("Internal server error");
            }
        }

        private static bool IsAdminArea(PathString path)
        {
            return path.StartsWithSegments("/admin", StringComparison.OrdinalIgnoreCase) ||
                   path.StartsWithSegments("/areas/admin", StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsAdminRole(UserRole role)
        {
            return role == UserRole.Admin;
        }

        private static bool IsSensitiveAdminOperation(PathString path, string method)
        {
            // Define sensitive operations that require higher privileges
            var sensitivePaths = new[]
            {
                "/admin/user/delete",
                "/admin/user/changerole",
                "/admin/system",
                "/admin/configuration",
                "/admin/reports/export"
            };

            var sensitiveOperations = new[]
            {
                "DELETE"
            };

            return sensitivePaths.Any(sp => path.StartsWithSegments(sp, StringComparison.OrdinalIgnoreCase)) ||
                   sensitiveOperations.Contains(method.ToUpperInvariant());
        }

        private static bool HasSensitiveAdminPermissions(UserRole role)
        {
            // Only SuperAdmin can perform sensitive operations
            return role == UserRole.Admin;
        }

        private async Task RedirectToLogin(HttpContext context)
        {
            if (IsApiRequest(context))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized");
            }
            else
            {
                context.Response.Redirect("/user/login?returnUrl=" + Uri.EscapeDataString(context.Request.Path));
            }
        }

        private async Task HandleForbidden(HttpContext context)
        {
            if (IsApiRequest(context))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Forbidden: Admin access required");
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Access Denied: Administrator privileges required");
            }
        }

        private static bool IsApiRequest(HttpContext context)
        {
            return context.Request.Path.StartsWithSegments("/api", StringComparison.OrdinalIgnoreCase) ||
                   context.Request.Headers["Accept"].ToString().Contains("application/json") ||
                   context.Request.Headers["Content-Type"].ToString().Contains("application/json");
        }
    }

    public static class AdminAuthorizationMiddlewareExtensions
    {
        public static IApplicationBuilder UseAdminAuthorization(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AdminAuthorizationMiddleware>();
        }
    }
}

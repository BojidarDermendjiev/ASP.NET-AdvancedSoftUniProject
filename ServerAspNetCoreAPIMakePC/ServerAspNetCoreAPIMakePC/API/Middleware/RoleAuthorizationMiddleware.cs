namespace ServerAspNetCoreAPIMakePC.API.Middleware
{
    using System.Security.Claims;

    using Application.Interfaces;

    public class RoleAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RoleAuthorizationMiddleware> _logger;

        public RoleAuthorizationMiddleware(RequestDelegate next, ILogger<RoleAuthorizationMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, IUserService userService)
        {
            try
            {
                // Skip authorization for public endpoints
                if (IsPublicEndpoint(context.Request.Path))
                {
                    await _next(context);
                    return;
                }

                // Check if user is authenticated
                if (!context.User.Identity.IsAuthenticated)
                {
                    _logger.LogWarning("Unauthorized access attempt to {Path}", context.Request.Path);
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Unauthorized");
                    return;
                }

                // Get user ID from claims (as Guid)
                var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
                {
                    _logger.LogWarning("Invalid user ID in claims for path {Path}", context.Request.Path);
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Invalid user credentials");
                    return;
                }

                // Get user from service to verify role
                var user = await userService.GetUserByIdAsync(userId);
                if (user == null)
                {
                    _logger.LogWarning("User {UserId} not found in database", userId);
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("User not found");
                    return;
                }

                context.Items["UserRole"] = user.Role;
                context.Items["UserId"] = userId;

                // _logger.LogDebug("User {UserId} with role {Role} accessing {Path}", userId, user.Role, context.Request.Path);

                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in RoleAuthorizationMiddleware for path {Path}", context.Request.Path);
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync("Internal server error");
            }
        }

        private static bool IsPublicEndpoint(PathString path)
        {
            var publicPaths = new[]
            {
                "/",
                "/home",
                "/product",
                "/brand",
                "/category",
                "/user/login",
                "/user/register",
                "/api/product",
                "/api/brand",
                "/api/category"
            };

            return publicPaths.Any(publicPath =>
                path.StartsWithSegments(publicPath, StringComparison.OrdinalIgnoreCase));
        }
    }

    public static class RoleAuthorizationMiddlewareExtensions
    {
        public static IApplicationBuilder UseRoleAuthorization(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RoleAuthorizationMiddleware>();
        }
    }
}
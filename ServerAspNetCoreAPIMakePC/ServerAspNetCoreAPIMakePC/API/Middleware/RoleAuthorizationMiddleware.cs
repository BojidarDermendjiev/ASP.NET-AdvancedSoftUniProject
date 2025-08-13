namespace ServerAspNetCoreAPIMakePC.API.Middleware
{
    using Application.Interfaces;
    using Microsoft.AspNetCore.Authorization;

    public class RoleAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RoleAuthorizationMiddleware> _logger;

        public RoleAuthorizationMiddleware(RequestDelegate next, ILogger<RoleAuthorizationMiddleware> logger)
        {
            this._next = next;
            this._logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, IUserService userService)
        {
            try
            {
                var endpoint = context.GetEndpoint();

                if (endpoint == null || endpoint.Metadata.GetMetadata<IAllowAnonymous>() != null)
                {
                    await this._next(context);
                    return;
                }

                if (context.Request.Path.StartsWithSegments("/favicon.ico") ||
                    context.Request.Path.StartsWithSegments("/.well-known"))
                {
                    await this._next(context);
                    return;
                }

                if (!context.User.Identity.IsAuthenticated)
                {
                    this._logger.LogWarning("Unauthorized access attempt to {Path}", context.Request.Path);
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Unauthorized");
                    return;
                }

                await this._next(context);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Error in RoleAuthorizationMiddleware for path {Path}", context.Request.Path);
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync("Internal server error");
            }
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
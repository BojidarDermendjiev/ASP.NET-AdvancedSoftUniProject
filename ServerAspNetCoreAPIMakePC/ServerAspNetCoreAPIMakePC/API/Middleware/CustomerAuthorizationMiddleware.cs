namespace ServerAspNetCoreAPIMakePC.API.Middleware
{
    using Domain.Enums;

    public class CustomerAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomerAuthorizationMiddleware> _logger;

        public CustomerAuthorizationMiddleware(RequestDelegate next, ILogger<CustomerAuthorizationMiddleware> logger)
        {
            this._next = next;
            this._logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Check if this is a customer-protected endpoint
                if (!IsCustomerProtectedEndpoint(context.Request.Path))
                {
                    await this._next(context);
                    return;
                }

                // Check if user is authenticated
                if (!context.User.Identity.IsAuthenticated)
                {
                    this._logger.LogWarning("Unauthenticated access attempt to customer area: {Path}", context.Request.Path);
                    await RedirectToLogin(context);
                    return;
                }

                // Get user role from context (set by RoleAuthorizationMiddleware)
                if (!context.Items.TryGetValue("UserRole", out var roleObj) || roleObj is not UserRole userRole)
                {
                    this._logger.LogWarning("No user role found in context for customer area access: {Path}", context.Request.Path);
                    await HandleUnauthorized(context);
                    return;
                }

                var userId = context.Items["UserId"];

                // Check if user has customer access (customers and admins can access customer areas)
                if (!HasCustomerAccess(userRole))
                {
                    this._logger.LogWarning("User {UserId} with role {Role} attempted to access customer area: {Path}",
                        userId, userRole, context.Request.Path);
                    await HandleForbidden(context);
                    return;
                }

                // Additional checks for specific customer operations
                if (IsResourceOwnershipRequired(context.Request.Path))
                {
                    if (!await ValidateResourceOwnership(context, userId))
                    {
                        this._logger.LogWarning("User {UserId} attempted to access resource they don't own: {Path}",
                            userId, context.Request.Path);
                        await HandleForbidden(context);
                        return;
                    }
                }

                // Check for shopping cart operations
                if (IsShoppingCartOperation(context.Request.Path))
                {
                    if (userRole != UserRole.User && userRole != UserRole.Admin && userRole != UserRole.Admin)
                    {
                        this._logger.LogWarning("User {UserId} with role {Role} attempted shopping cart operation: {Path}",
                            userId, userRole, context.Request.Path);
                        await HandleForbidden(context);
                        return;
                    }
                }

                this._logger.LogDebug("Customer access granted to user {UserId} with role {Role} for {Path}",
                    userId, userRole, context.Request.Path);

                await _next(context);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Error in CustomerAuthorizationMiddleware for path {Path}", context.Request.Path);
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync("Internal server error");
            }
        }

        private static bool IsCustomerProtectedEndpoint(PathString path)
        {
            var customerProtectedPaths = new[]
            {
                "/basket",
                "/shoppingcart",
                "/order",
                "/user/profile",
                "/user/changepassword",
                "/user/orders",
                "/review/create",
                "/review/update",
                "/review/delete",
                "/platformfeedback/create"
            };

            return customerProtectedPaths.Any(protectedPath =>
                path.StartsWithSegments(protectedPath, StringComparison.OrdinalIgnoreCase));
        }

        private static bool HasCustomerAccess(UserRole role)
        {
            // Customers, Admins, and SuperAdmins can access customer areas
            return role == UserRole.User ||
                   role == UserRole.Admin ||
                   role == UserRole.Moderator;
        }

        private static bool IsResourceOwnershipRequired(PathString path)
        {
            var ownershipRequiredPaths = new[]
            {
                "/order/",
                "/basket/",
                "/user/profile",
                "/user/changepassword",
                "/review/update",
                "/review/delete"
            };

            return ownershipRequiredPaths.Any(ownerPath =>
                path.ToString().Contains(ownerPath, StringComparison.OrdinalIgnoreCase));
        }

        private async Task<bool> ValidateResourceOwnership(HttpContext context, object userId)
        {
            // Skip ownership validation for admins
            if (context.Items.TryGetValue("UserRole", out var roleObj) && roleObj is UserRole role)
            {
                if (role == UserRole.Admin)
                {
                    return true;
                }
            }

            var path = context.Request.Path.ToString().ToLowerInvariant();
            var routeValues = context.Request.RouteValues;

            // Extract resource ID from route or query parameters
            string resourceId = null;

            if (routeValues.ContainsKey("id"))
            {
                resourceId = routeValues["id"]?.ToString();
            }
            else if (context.Request.Query.ContainsKey("id"))
            {
                resourceId = context.Request.Query["id"].FirstOrDefault();
            }

            // For paths that don't have specific resource IDs, allow access
            if (string.IsNullOrEmpty(resourceId))
            {
                return true;
            }

            // Here you would implement specific ownership validation logic
            // This is a simplified example - you'd need to inject appropriate services

            // Example: For order ownership validation
            if (path.Contains("/order/"))
            {
                // You would inject IOrderService and validate ownership
                // return await orderService.IsOrderOwnedByUserAsync(int.Parse(resourceId), (int)userId);
                return true; // Placeholder - implement actual validation
            }

            // Example: For basket ownership validation
            if (path.Contains("/basket/"))
            {
                // return await basketService.IsBasketOwnedByUserAsync(int.Parse(resourceId), (int)userId);
                return true; // Placeholder - implement actual validation
            }

            return true; // Default allow - implement specific validations as needed
        }

        private static bool IsShoppingCartOperation(PathString path)
        {
            var shoppingCartPaths = new[]
            {
                "/shoppingcart",
                "/basket/add",
                "/basket/remove",
                "/basket/update"
            };

            return shoppingCartPaths.Any(cartPath =>
                path.StartsWithSegments(cartPath, StringComparison.OrdinalIgnoreCase));
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

        private async Task HandleUnauthorized(HttpContext context)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync(IsApiRequest(context) ? "Unauthorized" : "Please log in to continue");
        }

        private async Task HandleForbidden(HttpContext context)
        {
            if (IsApiRequest(context))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Forbidden: Insufficient permissions");
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Access Denied: You don't have permission to access this resource");
            }
        }

        private static bool IsApiRequest(HttpContext context)
        {
            return context.Request.Path.StartsWithSegments("/api", StringComparison.OrdinalIgnoreCase) ||
                   context.Request.Headers["Accept"].ToString().Contains("application/json") ||
                   context.Request.Headers["Content-Type"].ToString().Contains("application/json");
        }
    }

    public static class CustomerAuthorizationMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomerAuthorization(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomerAuthorizationMiddleware>();
        }
    }
}

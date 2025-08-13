namespace ServerAspNetCoreAPIMakePC.API.Middleware
{
    public class MaintenanceModeMiddleware
    {
        private readonly RequestDelegate _next;
        private static bool _isInMaintenance = false;

        public MaintenanceModeMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (_isInMaintenance)
            {
                context.Response.StatusCode = 503;
                await context.Response.WriteAsync("API is in maintenance mode.");
                return;
            }
            await _next(context);
        }

        // Call this somewhere to enable/disable maintenance
        public static void SetMaintenance(bool isInMaintenance) => _isInMaintenance = isInMaintenance;
    }
}

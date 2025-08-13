namespace ServerAspNetCoreAPIMakePC.API.Middleware
{
    public class CustomHeaderMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomHeaderMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Example: Add a custom header to the response
            context.Response.OnStarting(() =>
            {
                context.Response.Headers["X-Powered-By"] = "MakePC-API";
                context.Response.Headers["X-Api-Version"] = "1.0";
                return Task.CompletedTask;
            });

            await _next(context);
        }
    }
}

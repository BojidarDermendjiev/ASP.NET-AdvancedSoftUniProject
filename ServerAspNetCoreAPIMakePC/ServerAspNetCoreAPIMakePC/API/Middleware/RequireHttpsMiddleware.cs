namespace ServerAspNetCoreAPIMakePC.API.Middleware
{
    using System.Net;

    public class RequireHttpsMiddleware
    {
        private readonly RequestDelegate _next;

        public RequireHttpsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.IsHttps)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                await context.Response.WriteAsync("HTTPS Required.");
                return;
            }
            await _next(context);
        }
    }
}

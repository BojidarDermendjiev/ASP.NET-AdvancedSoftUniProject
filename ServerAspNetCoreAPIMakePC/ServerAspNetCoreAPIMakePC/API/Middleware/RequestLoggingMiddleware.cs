namespace ServerAspNetCoreAPIMakePC.API.Middleware
{
    using System.Diagnostics;

    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var sw = Stopwatch.StartNew();
            var request = context.Request;
            var method = request.Method;
            var path = request.Path;

            await this._next(context);

            sw.Stop();
            var statusCode = context.Response.StatusCode;
            Console.WriteLine($"{method} {path} responded {statusCode} in {sw.ElapsedMilliseconds} ms");
        }
    }
}

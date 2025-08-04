namespace ServerAspNetCoreAPIMakePC.API.Middleware
{
    using System.Collections.Concurrent;

    public class RateLimitingMiddleware
    {
        private readonly RequestDelegate _next;
        private static readonly ConcurrentDictionary<string, (int Count, DateTime Window)> _requests = new();
        private readonly int _maxRequests;
        private readonly TimeSpan _window;

        public RateLimitingMiddleware(RequestDelegate next, int maxRequests = 60, int windowSeconds = 60)
        {
            _next = next;
            _maxRequests = maxRequests;
            _window = TimeSpan.FromSeconds(windowSeconds);
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var key = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var now = DateTime.UtcNow;

            var entry = _requests.GetOrAdd(key, _ => (0, now));
            if (now - entry.Window > _window)
            {
                _requests[key] = (1, now);
            }
            else
            {
                if (entry.Count >= _maxRequests)
                {
                    context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    context.Response.Headers["Retry-After"] = _window.TotalSeconds.ToString();
                    await context.Response.WriteAsync("Rate limit exceeded. Try again later.");
                    return;
                }
                _requests[key] = (entry.Count + 1, entry.Window);
            }

            await _next(context);
        }
    }
}

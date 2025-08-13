public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string _apiKey;
    private readonly string _apiKeyHeaderName;

    public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        this._next = next;
        this._apiKey = configuration["ApiKeyAuth:ApiKey"] ?? throw new InvalidOperationException("API Key not configured!");
        this._apiKeyHeaderName = configuration["ApiKeyAuth:ApiKeyHeaderName"] ?? throw new InvalidOperationException("API Key Header Name not configured!");
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue(this._apiKeyHeaderName, out var extractedApiKey) ||
            !string.Equals(extractedApiKey, this._apiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("API Key is missing or invalid.");
            return;
        }

        await this._next(context);
    }
}

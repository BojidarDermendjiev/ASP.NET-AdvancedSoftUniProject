namespace ServerAspNetCoreAPIMakePC.Tests.Middleware
{
    using Moq;
    using NUnit.Framework;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;

    using API.Middleware;


    [TestFixture]
    public class ApiKeyMiddlewareTests
    {
        private const string ApiKey = "test-api-key";
        private const string ApiKeyHeaderName = "X-API-KEY";

        private ApiKeyMiddleware CreateMiddleware(RequestDelegate next)
        {
            var inMemorySettings = new Dictionary<string, string>
            {
                {
                    "ApiKeyAuth:ApiKey",
                    ApiKey
                },
                {
                    "ApiKeyAuth:ApiKeyHeaderName",
                    ApiKeyHeaderName
                }
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            return new ApiKeyMiddleware(next, configuration);
        }

        [Test]
        public async Task InvokeAsync_WithValidApiKey_CallsNext()
        {
            var context = new DefaultHttpContext();
            context.Request.Headers[ApiKeyHeaderName] = ApiKey;

            bool nextCalled = false;
            RequestDelegate next = ctx =>
            {
                nextCalled = true;
                return Task.CompletedTask;
            };

            var middleware = CreateMiddleware(next);

            await middleware.InvokeAsync(context);

            Assert.IsTrue(nextCalled);
            Assert.AreNotEqual(401, context.Response.StatusCode);
        }

        [Test]
        public async Task InvokeAsync_MissingApiKey_Returns401()
        {
            var context = new DefaultHttpContext();

            RequestDelegate next = ctx =>
            {
                Assert.Fail("Next middleware should not be called.");
                return Task.CompletedTask;
            };

            var middleware = CreateMiddleware(next);

            await middleware.InvokeAsync(context);

            Assert.AreEqual(401, context.Response.StatusCode);
        }

        [Test]
        public async Task InvokeAsync_InvalidApiKey_Returns401()
        {
            var context = new DefaultHttpContext();
            context.Request.Headers[ApiKeyHeaderName] = "wrong-key";

            RequestDelegate next = ctx =>
            {
                Assert.Fail("Next middleware should not be called.");
                return Task.CompletedTask;
            };

            var middleware = CreateMiddleware(next);

            await middleware.InvokeAsync(context);

            Assert.AreEqual(401, context.Response.StatusCode);
        }
    }
}

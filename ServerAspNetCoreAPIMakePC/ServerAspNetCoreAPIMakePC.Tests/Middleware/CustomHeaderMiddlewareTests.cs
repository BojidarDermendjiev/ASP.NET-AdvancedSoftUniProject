namespace ServerAspNetCoreAPIMakePC.Tests.Middleware
{
    using NUnit.Framework;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    
    using API.Middleware;

    [TestFixture]
    public class CustomHeaderMiddlewareTests
    {
        [Test]
        public async Task InvokeAsync_AddsCustomHeadersToResponse()
        {
            var context = new DefaultHttpContext();
            bool nextCalled = false;

            RequestDelegate next = ctx =>
            {
                nextCalled = true;
                return Task.CompletedTask;
            };

            var middleware = new CustomHeaderMiddleware(next);

            await middleware.InvokeAsync(context);

            await context.Response.StartAsync();

            Assert.IsTrue(nextCalled);
            Assert.IsTrue(context.Response.Headers.ContainsKey("X-Powered-By"));
            Assert.AreEqual("MakePC-API", context.Response.Headers["X-Powered-By"]);
            Assert.IsTrue(context.Response.Headers.ContainsKey("X-Api-Version"));
            Assert.AreEqual("1.0", context.Response.Headers["X-Api-Version"]);
        }
    }
}
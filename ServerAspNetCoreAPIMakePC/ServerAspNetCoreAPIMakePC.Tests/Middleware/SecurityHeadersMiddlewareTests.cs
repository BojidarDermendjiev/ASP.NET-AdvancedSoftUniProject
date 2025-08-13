namespace ServerAspNetCoreAPIMakePC.Tests.Middleware
{

    using Moq;
    using NUnit.Framework;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;

    using API.Middleware;

    [TestFixture]
    public class SecurityHeadersMiddlewareTests
    {
        [Test]
        public async Task InvokeAsync_AddsExpectedSecurityHeaders()
        {
            var context = new DefaultHttpContext();
            var nextInvoked = false;
            RequestDelegate next = ctx =>
            {
                nextInvoked = true;
                return Task.CompletedTask;
            };
            var middleware = new SecurityHeadersMiddleware(next);

            await middleware.InvokeAsync(context);

            await context.Response.StartAsync();

            Assert.IsTrue(nextInvoked, "Next middleware was not invoked");
            var headers = context.Response.Headers;
            Assert.AreEqual("nosniff", headers["X-Content-Type-Options"]);
            Assert.AreEqual("DENY", headers["X-Frame-Options"]);
            Assert.AreEqual("1; mode=block", headers["X-XSS-Protection"]);
            Assert.AreEqual("strict-origin-when-cross-origin", headers["Referrer-Policy"]);
            Assert.AreEqual("default-src 'self'; style-src 'self' 'unsafe-inline';", headers["Content-Security-Policy"]);
            Assert.AreEqual("max-age=31536000; includeSubDomains", headers["Strict-Transport-Security"]);
        }
    }
}
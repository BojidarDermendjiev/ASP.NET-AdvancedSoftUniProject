namespace ServerAspNetCoreAPIMakePC.Tests.Middleware
{
    using NUnit.Framework;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;

    using API.Middleware;

    [TestFixture]
    public class CorrelationIdMiddlewareTests
    {
        private const string CorrelationIdHeader = "X-Correlation-ID";

        private CorrelationIdMiddleware CreateMiddleware(RequestDelegate next)
        {
            return new CorrelationIdMiddleware(next);
        }

        [Test]
        public async Task InvokeAsync_WhenHeaderExists_PreservesCorrelationId()
        {
            var context = new DefaultHttpContext();
            var expectedCorrelationId = "existing-correlation-id";
            context.Request.Headers[CorrelationIdHeader] = expectedCorrelationId;

            bool nextCalled = false;
            RequestDelegate next = ctx =>
            {
                nextCalled = true;
                return Task.CompletedTask;
            };

            var middleware = CreateMiddleware(next);

            await middleware.InvokeAsync(context);

            Assert.IsTrue(nextCalled);
            Assert.IsTrue(context.Response.Headers.ContainsKey(CorrelationIdHeader));
            Assert.AreEqual(expectedCorrelationId, context.Response.Headers[CorrelationIdHeader]);
        }

        [Test]
        public async Task InvokeAsync_WhenHeaderMissing_GeneratesCorrelationId()
        {
            var context = new DefaultHttpContext();

            bool nextCalled = false;
            RequestDelegate next = ctx =>
            {
                nextCalled = true;
                return Task.CompletedTask;
            };

            var middleware = CreateMiddleware(next);

            await middleware.InvokeAsync(context);

            Assert.IsTrue(nextCalled);
            Assert.IsTrue(context.Request.Headers.ContainsKey(CorrelationIdHeader));
            Assert.IsTrue(context.Response.Headers.ContainsKey(CorrelationIdHeader));
            var requestCorrelationId = context.Request.Headers[CorrelationIdHeader].ToString();
            var responseCorrelationId = context.Response.Headers[CorrelationIdHeader].ToString();
            Assert.IsNotEmpty(requestCorrelationId);
            Assert.IsNotEmpty(responseCorrelationId);
            Assert.AreEqual(requestCorrelationId, responseCorrelationId);
        }
    }
}
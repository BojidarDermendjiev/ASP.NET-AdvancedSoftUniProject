namespace ServerAspNetCoreAPIMakePC.Tests.Middleware
{
    using NUnit.Framework;
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    
    using API.Middleware;

    [TestFixture]
    public class RateLimitingMiddlewareTests
    {
        private DefaultHttpContext CreateContext(string ip = "127.0.0.1")
        {
            var context = new DefaultHttpContext();
            context.Connection.RemoteIpAddress = System.Net.IPAddress.Parse(ip);
            return context;
        }

        [SetUp]
        public void SetUp()
        {
            typeof(RateLimitingMiddleware)
                .GetField("_requests", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic)
                ?.SetValue(null, new System.Collections.Concurrent.ConcurrentDictionary<string, (int, DateTime)>());
        }

        [Test]
        public async Task InvokeAsync_UnderLimit_AllowsRequest()
        {
            var context = CreateContext();
            bool nextCalled = false;
            RequestDelegate next = ctx =>
            {
                nextCalled = true;
                return Task.CompletedTask;
            };

            var middleware = new RateLimitingMiddleware(next, maxRequests: 2, windowSeconds: 60);

            await middleware.InvokeAsync(context);

            Assert.IsTrue(nextCalled);
            Assert.AreEqual(200, context.Response.StatusCode == 0 ? 200 : context.Response.StatusCode);
        }

        [Test]
        public async Task InvokeAsync_ExceedsLimit_Returns429()
        {
            var context1 = CreateContext();
            var context2 = CreateContext();
            var context3 = CreateContext();

            var middleware = new RateLimitingMiddleware(_ => Task.CompletedTask, maxRequests: 2, windowSeconds: 60);

            await middleware.InvokeAsync(context1);
            await middleware.InvokeAsync(context2);

            bool nextCalled = false;
            RequestDelegate next = ctx =>
            {
                nextCalled = true;
                return Task.CompletedTask;
            };

            var middleware2 = new RateLimitingMiddleware(next, maxRequests: 2, windowSeconds: 60);

            await middleware2.InvokeAsync(context3);

            Assert.IsFalse(nextCalled);
            Assert.AreEqual(StatusCodes.Status429TooManyRequests, context3.Response.StatusCode);
            Assert.IsTrue(context3.Response.Headers.ContainsKey("Retry-After"));

            context3.Response.Body.Seek(0, System.IO.SeekOrigin.Begin);
            string responseBody = await new System.IO.StreamReader(context3.Response.Body).ReadToEndAsync();
            Assert.AreEqual("Rate limit exceeded. Try again later.", responseBody);
        }

        [Test]
        public async Task InvokeAsync_NewWindow_ResetsCounter()
        {
            var context = CreateContext();
            var now = DateTime.UtcNow;

            var middleware = new RateLimitingMiddleware(_ => Task.CompletedTask, maxRequests: 1, windowSeconds: 1);

            await middleware.InvokeAsync(context);

            var field = typeof(RateLimitingMiddleware).GetField("_requests", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

            var requestsDictObj = field?.GetValue(null);

            if (requestsDictObj is System.Collections.Concurrent.ConcurrentDictionary<string, (int, DateTime)> dict
                && dict.TryGetValue("127.0.0.1", out var entry))
            {
                dict["127.0.0.1"] = (entry.Item1, now - TimeSpan.FromSeconds(2));
            }

            bool nextCalled = false;
            RequestDelegate next = ctx =>
            {
                nextCalled = true;
                return Task.CompletedTask;
            };
            var middleware2 = new RateLimitingMiddleware(next, maxRequests: 1, windowSeconds: 1);
            await middleware2.InvokeAsync(context);

            Assert.IsTrue(nextCalled);
            Assert.AreEqual(200, context.Response.StatusCode == 0 ? 200 : context.Response.StatusCode);
        }
    }
}
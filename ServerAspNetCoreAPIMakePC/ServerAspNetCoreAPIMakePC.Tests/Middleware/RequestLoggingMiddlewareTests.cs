namespace ServerAspNetCoreAPIMakePC.Tests.Middleware
{
    using System;
    using System.IO;
    using NUnit.Framework;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    
    using API.Middleware;

    [TestFixture]
    public class RequestLoggingMiddlewareTests
    {
        [Test]
        public async Task InvokeAsync_LogsRequestInformationToConsole()
        {
            var context = new DefaultHttpContext();
            context.Request.Method = "GET";
            context.Request.Path = "/api/test";
            context.Response.StatusCode = 204;

            bool nextCalled = false;
            RequestDelegate next = ctx =>
            {
                nextCalled = true;
                return Task.CompletedTask;
            };

            var middleware = new RequestLoggingMiddleware(next);

            using (var sw = new StringWriter())
            {
                var originalOut = Console.Out;
                Console.SetOut(sw);

                await middleware.InvokeAsync(context);

                Console.SetOut(originalOut);

                Assert.IsTrue(nextCalled);
                var output = sw.ToString();
                StringAssert.Contains("GET /api/test responded 204 in", output);
                StringAssert.Contains("ms", output);
            }
        }
    }
}
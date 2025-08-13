namespace ServerAspNetCoreAPIMakePC.Tests.Middleware
{
    using System;
    using System.Net;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using NUnit.Framework;

    using API.Middleware;

    [TestFixture]
    public class GlobalExceptionMiddlewareTests
    {
        [Test]
        public async Task InvokeAsync_NoException_CallsNext()
        {
            var context = new DefaultHttpContext();
            bool nextCalled = false;
            RequestDelegate next = ctx =>
            {
                nextCalled = true;
                return Task.CompletedTask;
            };

            var middleware = new GlobalExceptionMiddleware(next);

            await middleware.InvokeAsync(context);

            Assert.IsTrue(nextCalled);
            Assert.AreEqual(200, context.Response.StatusCode == 0 ? 200 : context.Response.StatusCode);
        }

        [Test]
        public async Task InvokeAsync_ExceptionThrown_ReturnsInternalServerErrorAndJson()
        {
            var context = new DefaultHttpContext();
            var expectedException = new InvalidOperationException("Test exception");
            RequestDelegate next = ctx => throw expectedException;

            var middleware = new GlobalExceptionMiddleware(next);

            await middleware.InvokeAsync(context);

            Assert.AreEqual((int)HttpStatusCode.InternalServerError, context.Response.StatusCode);
            Assert.AreEqual("application/json", context.Response.ContentType);

            context.Response.Body.Seek(0, System.IO.SeekOrigin.Begin);
            string responseBody = await new System.IO.StreamReader(context.Response.Body).ReadToEndAsync();

            var json = JsonSerializer.Deserialize<JsonElement>(responseBody);

            Assert.IsTrue(json.TryGetProperty("error", out var errorProp));
            Assert.AreEqual(expectedException.Message, errorProp.GetString());
        }
    }
}
namespace ServerAspNetCoreAPIMakePC.Tests.Middleware
{
    using System;
    using System.Net;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Moq;
    using NUnit.Framework;

    using API.Middleware;

    [TestFixture]
    public class ErrorHandlingMiddlewareTests
    {
        private Mock<ILogger<ErrorHandlingMiddleware>> _loggerMock;

        [SetUp]
        public void SetUp()
        {
            this._loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
        }

        private ErrorHandlingMiddleware CreateMiddleware(RequestDelegate next)
        {
            return new ErrorHandlingMiddleware(next, this._loggerMock.Object);
        }

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

            var middleware = CreateMiddleware(next);

            await middleware.InvokeAsync(context);

            Assert.IsTrue(nextCalled);
            Assert.AreEqual(200, context.Response.StatusCode == 0 ? 200 : context.Response.StatusCode);
        }

        [Test]
        public async Task InvokeAsync_WithException_ReturnsInternalServerErrorAndJson()
        {
            var context = new DefaultHttpContext();
            var expectedException = new InvalidOperationException("Test exception");
            RequestDelegate next = ctx => throw expectedException;

            var middleware = CreateMiddleware(next);

            await middleware.InvokeAsync(context);

            Assert.AreEqual((int)HttpStatusCode.InternalServerError, context.Response.StatusCode);
            Assert.AreEqual("application/json", context.Response.ContentType);

            context.Response.Body.Seek(0, System.IO.SeekOrigin.Begin);
            string responseBody = await new System.IO.StreamReader(context.Response.Body).ReadToEndAsync();

            var json = JsonSerializer.Deserialize<JsonElement>(responseBody);

            Assert.IsTrue(json.TryGetProperty("error", out var errorProp));
            Assert.AreEqual("An unexpected error occurred.", errorProp.GetString());
            Assert.IsTrue(json.TryGetProperty("details", out var detailsProp));
            Assert.AreEqual(expectedException.Message, detailsProp.GetString());

            this._loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Unhandled exception")),
                    expectedException,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }
    }
}
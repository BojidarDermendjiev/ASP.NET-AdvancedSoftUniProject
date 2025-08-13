namespace ServerAspNetCoreAPIMakePC.Tests.Middleware
{
    using System;
    using NUnit.Framework;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Logging;
    using Microsoft.AspNetCore.Authorization;

    using Moq;
   
    using API.Middleware;
    using Application.Interfaces;

    [TestFixture]
    public class RoleAuthorizationMiddlewareTests
    {
        private Mock<ILogger<RoleAuthorizationMiddleware>> _loggerMock;
        private Mock<IUserService> _userServiceMock;

        [SetUp]
        public void SetUp()
        {
            this._loggerMock = new Mock<ILogger<RoleAuthorizationMiddleware>>();
            this._userServiceMock = new Mock<IUserService>();
        }

        private RoleAuthorizationMiddleware CreateMiddleware(RequestDelegate next)
        {
            return new RoleAuthorizationMiddleware(next, this._loggerMock.Object);
        }

        private static void SetAllowAnonymousEndpoint(HttpContext context)
        {
            var endpoint = new Endpoint(
                requestDelegate: (ctx) => Task.CompletedTask,
                new EndpointMetadataCollection(new IAllowAnonymous[] { new AllowAnonymousAttribute() }),
                "test"
            );
            context.SetEndpoint(endpoint);
        }

        private static void SetNormalEndpoint(HttpContext context)
        {
            var endpoint = new Endpoint(
                requestDelegate: (ctx) => Task.CompletedTask,
                new EndpointMetadataCollection(),
                "test"
            );
            context.SetEndpoint(endpoint);
        }

        [Test]
        public async Task InvokeAsync_AllowsAnonymous_SkipsAuthorization()
        {
            var context = new DefaultHttpContext();
            SetAllowAnonymousEndpoint(context);

            bool nextCalled = false;
            RequestDelegate next = (ctx) =>
            {
                nextCalled = true;
                return Task.CompletedTask;
            };

            var middleware = CreateMiddleware(next);

            await middleware.InvokeAsync(context, this._userServiceMock.Object);

            Assert.IsTrue(nextCalled);
        }

        [Test]
        public async Task InvokeAsync_PathIsFaviconOrWellKnown_SkipsAuthorization()
        {
            var paths = new[] { "/favicon.ico", "/.well-known/something" };
            foreach (var path in paths)
            {
                var context = new DefaultHttpContext();
                context.Request.Path = path;
                SetNormalEndpoint(context);

                bool nextCalled = false;
                RequestDelegate next = (ctx) =>
                {
                    nextCalled = true;
                    return Task.CompletedTask;
                };

                var middleware = CreateMiddleware(next);

                // Act
                await middleware.InvokeAsync(context, _userServiceMock.Object);

                // Assert
                Assert.IsTrue(nextCalled, $"Path {path} should skip authorization.");
            }
        }

        [Test]
        public async Task InvokeAsync_NotAuthenticated_Returns401AndLogs()
        {
            var context = new DefaultHttpContext();
            context.Request.Path = "/api/secure";
            SetNormalEndpoint(context);

            context.User = new ClaimsPrincipal(new ClaimsIdentity()); 

            bool nextCalled = false;
            RequestDelegate next = (ctx) =>
            {
                nextCalled = true;
                return Task.CompletedTask;
            };

            var middleware = CreateMiddleware(next);

            await middleware.InvokeAsync(context, _userServiceMock.Object);

            Assert.IsFalse(nextCalled);
            Assert.AreEqual(StatusCodes.Status401Unauthorized, context.Response.StatusCode);

            context.Response.Body.Seek(0, System.IO.SeekOrigin.Begin);
            string responseBody = await new System.IO.StreamReader(context.Response.Body).ReadToEndAsync();
            Assert.AreEqual("Unauthorized", responseBody);

            this._loggerMock.Verify(
                l => l.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Unauthorized access attempt")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once
            );
        }

        [Test]
        public async Task InvokeAsync_Authenticated_CallsNext()
        {
            var context = new DefaultHttpContext();
            context.Request.Path = "/api/secure";
            SetNormalEndpoint(context);

            var claimsIdentity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, "user") }, "TestAuthType");
            context.User = new ClaimsPrincipal(claimsIdentity);

            bool nextCalled = false;
            RequestDelegate next = (ctx) =>
            {
                nextCalled = true;
                return Task.CompletedTask;
            };

            var middleware = CreateMiddleware(next);

            await middleware.InvokeAsync(context, this._userServiceMock.Object);

            Assert.IsTrue(nextCalled);
        }

        [Test]
        public async Task InvokeAsync_Exception_Returns500AndLogs()
        {
            var context = new DefaultHttpContext();
            context.Request.Path = "/api/secure";
            SetNormalEndpoint(context);

            var claimsIdentity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, "user") }, "TestAuthType");
            context.User = new ClaimsPrincipal(claimsIdentity);

            RequestDelegate next = ctx => throw new Exception("Test error");
            var middleware = CreateMiddleware(next);

            await middleware.InvokeAsync(context, _userServiceMock.Object);

            Assert.AreEqual(StatusCodes.Status500InternalServerError, context.Response.StatusCode);

            context.Response.Body.Seek(0, System.IO.SeekOrigin.Begin);
            string responseBody = await new System.IO.StreamReader(context.Response.Body).ReadToEndAsync();
            Assert.AreEqual("Internal server error", responseBody);

            this._loggerMock.Verify(
                l => l.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error in RoleAuthorizationMiddleware")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once
            );
        }
    }
}
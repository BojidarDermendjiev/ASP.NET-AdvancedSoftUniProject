namespace ServerAspNetCoreAPIMakePC.Tests.Middleware
{
    using Moq;
    using NUnit.Framework;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using System.Collections.Generic;
    using Microsoft.Extensions.Logging;

    using Domain.Enums;
    using API.Middleware;

    [TestFixture]
    public class CustomerAuthorizationMiddlewareTests
    {
        private Mock<ILogger<CustomerAuthorizationMiddleware>> _loggerMock;

        [SetUp]
        public void SetUp()
        {
            this._loggerMock = new Mock<ILogger<CustomerAuthorizationMiddleware>>();
        }

        private CustomerAuthorizationMiddleware CreateMiddleware(RequestDelegate next)
        {
            return new CustomerAuthorizationMiddleware(next,this._loggerMock.Object);
        }

        private static ClaimsPrincipal CreatePrincipal(bool authenticated, UserRole? role = null)
        {
            var identity = new ClaimsIdentity();
            if (authenticated)
                identity = new ClaimsIdentity("TestAuthType", "name", "role");

            var principal = new ClaimsPrincipal(identity);

            return principal;
        }

        [Test]
        public async Task InvokeAsync_NonCustomerProtectedEndpoint_CallsNext()
        {
            var context = new DefaultHttpContext();
            context.Request.Path = "/public";
            bool called = false;
            var middleware = CreateMiddleware(_ =>
            {
                called = true;
                return Task.CompletedTask;
            });

            await middleware.InvokeAsync(context);

            Assert.IsTrue(called);
            Assert.AreEqual(200, context.Response.StatusCode == 0 ? 200 : context.Response.StatusCode); 
        }

        [Test]
        public async Task InvokeAsync_UnauthenticatedUser_OnProtectedEndpoint_ReturnsUnauthorized()
        {
            var context = new DefaultHttpContext();
            context.Request.Path = "/basket";
            context.User = new ClaimsPrincipal(new ClaimsIdentity()); 

            var middleware = CreateMiddleware(_ => Task.CompletedTask);

            await middleware.InvokeAsync(context);

            Assert.AreEqual(StatusCodes.Status401Unauthorized, context.Response.StatusCode);
        }

        [Test]
        public async Task InvokeAsync_NoUserRole_OnProtectedEndpoint_ReturnsUnauthorized()
        {
            var context = new DefaultHttpContext();
            context.Request.Path = "/basket";
            context.User = new ClaimsPrincipal(new ClaimsIdentity("TestAuthType"));

            var middleware = CreateMiddleware(_ => Task.CompletedTask);

            await middleware.InvokeAsync(context);

            Assert.AreEqual(StatusCodes.Status401Unauthorized, context.Response.StatusCode);
        }

        [Test]
        public async Task InvokeAsync_UserWithNoCustomerAccess_ReturnsForbidden()
        {
            var context = new DefaultHttpContext();
            context.Request.Path = "/basket";
            context.User = new ClaimsPrincipal(new ClaimsIdentity("TestAuthType"));
            context.Items["UserRole"] = UserRole.User; 
            context.Items["UserId"] = 123;

            var middleware = CreateMiddleware(_ => Task.CompletedTask);

            await middleware.InvokeAsync(context);

            Assert.AreEqual(StatusCodes.Status403Forbidden, context.Response.StatusCode);
        }

        [Test]
        public async Task InvokeAsync_UserWithCustomerAccess_AllowsAccess()
        {
            var context = new DefaultHttpContext();
            context.Request.Path = "/basket";
            context.User = new ClaimsPrincipal(new ClaimsIdentity("TestAuthType"));
            context.Items["UserRole"] = UserRole.User; // Allowed
            context.Items["UserId"] = 99;

            bool called = false;
            var middleware = CreateMiddleware(_ =>
            {
                called = true;
                return Task.CompletedTask;
            });

            await middleware.InvokeAsync(context);

            Assert.IsTrue(called);
            Assert.AreEqual(200, context.Response.StatusCode == 0 ? 200 : context.Response.StatusCode);
        }

        [Test]
        public async Task InvokeAsync_AdminCanAccessResourceOwnershipEndpoint()
        {
            var context = new DefaultHttpContext();
            context.Request.Path = "/order/12";
            context.User = new ClaimsPrincipal(new ClaimsIdentity("TestAuthType"));
            context.Items["UserRole"] = UserRole.Admin;
            context.Items["UserId"] = 42;
            context.Request.RouteValues["id"] = "12";

            bool called = false;
            var middleware = CreateMiddleware(_ =>
            {
                called = true;
                return Task.CompletedTask;
            });

            await middleware.InvokeAsync(context);

            Assert.IsTrue(called);
        }

        [Test]
        public async Task InvokeAsync_ShoppingCart_WithNonCustomerRole_ReturnsForbidden()
        {
            var context = new DefaultHttpContext();
            context.Request.Path = "/shoppingcart";
            context.User = new ClaimsPrincipal(new ClaimsIdentity("TestAuthType"));
            context.Items["UserRole"] = UserRole.Admin;
            context.Items["UserId"] = 77;

            var middleware = CreateMiddleware(_ => Task.CompletedTask);

            await middleware.InvokeAsync(context);

            Assert.AreEqual(StatusCodes.Status403Forbidden, context.Response.StatusCode);
        }
    }
}
namespace ServerAspNetCoreAPIMakePC.Tests.Middleware
{
    using System.Net;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using NUnit.Framework;
    
    using API.Middleware;

    [TestFixture]
    public class RequireHttpsMiddlewareTests
    {
        [Test]
        public async Task InvokeAsync_HttpsRequest_CallsNext()
        {
            var context = new DefaultHttpContext();
            context.Request.Scheme = "https";
            Assert.IsTrue(context.Request.IsHttps);

            bool nextCalled = false;
            RequestDelegate next = ctx =>
            {
                nextCalled = true;
                return Task.CompletedTask;
            };

            var middleware = new RequireHttpsMiddleware(next);

            await middleware.InvokeAsync(context);

            Assert.IsTrue(nextCalled);
            Assert.AreEqual(200, context.Response.StatusCode == 0 ? 200 : context.Response.StatusCode);
        }

        [Test]
        public async Task InvokeAsync_HttpRequest_ReturnsForbidden()
        {
            var context = new DefaultHttpContext();
            context.Request.Scheme = "http";
            Assert.IsFalse(context.Request.IsHttps);

            bool nextCalled = false;
            RequestDelegate next = ctx =>
            {
                nextCalled = true;
                return Task.CompletedTask;
            };

            var middleware = new RequireHttpsMiddleware(next);

            await middleware.InvokeAsync(context);

            Assert.IsFalse(nextCalled);
            Assert.AreEqual((int)HttpStatusCode.Forbidden, context.Response.StatusCode);

            context.Response.Body.Seek(0, System.IO.SeekOrigin.Begin);
            string responseBody = await new System.IO.StreamReader(context.Response.Body).ReadToEndAsync();
            Assert.AreEqual("HTTPS Required.", responseBody);
        }
    }
}
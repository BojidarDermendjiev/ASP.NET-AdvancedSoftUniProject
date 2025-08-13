namespace ServerAspNetCoreAPIMakePC.Tests.Middleware
{
    using NUnit.Framework;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;

    using API.Middleware;
    
    [TestFixture]
    public class MaintenanceModeMiddlewareTests
    {
        [SetUp]
        public void SetUp()
        {
            MaintenanceModeMiddleware.SetMaintenance(false);
        }

        [TearDown]
        public void TearDown()
        {
            MaintenanceModeMiddleware.SetMaintenance(false);
        }

        [Test]
        public async Task InvokeAsync_MaintenanceModeOff_CallsNext()
        {
            MaintenanceModeMiddleware.SetMaintenance(false);
            var context = new DefaultHttpContext();
            bool nextCalled = false;
            RequestDelegate next = ctx =>
            {
                nextCalled = true;
                return Task.CompletedTask;
            };

            var middleware = new MaintenanceModeMiddleware(next);

            await middleware.InvokeAsync(context);

            Assert.IsTrue(nextCalled);
            Assert.AreEqual(200, context.Response.StatusCode == 0 ? 200 : context.Response.StatusCode);
        }

        [Test]
        public async Task InvokeAsync_MaintenanceModeOn_Returns503AndMessage()
        {
            MaintenanceModeMiddleware.SetMaintenance(true);
            var context = new DefaultHttpContext();
            bool nextCalled = false;
            RequestDelegate next = ctx =>
            {
                nextCalled = true;
                return Task.CompletedTask;
            };

            var middleware = new MaintenanceModeMiddleware(next);

            await middleware.InvokeAsync(context);

            Assert.IsFalse(nextCalled);
            Assert.AreEqual(503, context.Response.StatusCode);

            context.Response.Body.Seek(0, System.IO.SeekOrigin.Begin);
            string responseBody = await new System.IO.StreamReader(context.Response.Body).ReadToEndAsync();
            Assert.AreEqual("API is in maintenance mode.", responseBody);
        }
    }
}
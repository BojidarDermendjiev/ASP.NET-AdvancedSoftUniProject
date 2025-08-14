namespace ServerAspNetCoreAPIMakePC.Tests.Integration
{
    using System.Net;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc.Testing;

    using NUnit.Framework;
    public class StartupTests
    {
        private WebApplicationFactory<ServerAspNetCoreAPIMakePC.API.StartUp> _factory;

        [SetUp]
        public void SetUp()
        {
            this._factory = new WebApplicationFactory<ServerAspNetCoreAPIMakePC.API.StartUp>();
        }

        [Test]
        public async Task GetSwaggerUi_ReturnsOk()
        {
            var client = this._factory.CreateClient();
            var response = await client.GetAsync("/swagger/index.html");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task CorsPolicy_AllowsAnyOrigin()
        {
            var client = this._factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
            var request = new HttpRequestMessage(HttpMethod.Options, "/api/anyendpoint");

            request.Headers.Add("Origin", "http://example.com");
            request.Headers.Add("Access-Control-Request-Method", "GET");

            var response = await client.SendAsync(request);
            Assert.That(response.Headers.Contains("Access-Control-Allow-Origin"));
        }

        [Test]
        public async Task StatusCodePages_ReturnCustomJson()
        {
            var client = this._factory.CreateClient();
            var response = await client.GetAsync("/api/thisdoesnotexist");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            var content = await response.Content.ReadAsStringAsync();
            StringAssert.Contains("Resource not found", content);
        }

        [TearDown]
        public void TearDown()
        {
            this._factory.Dispose();
        }
    }
}
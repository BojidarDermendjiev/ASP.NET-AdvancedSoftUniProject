namespace ServerAspNetCoreAPIMakePC.Tests.Services
{
    using Moq;
    using System.Net;
    using Moq.Protected;
    using System.Text.Json;

    using ServerAspNetCoreAPIMakePC.Infrastructure.Models;
    using ServerAspNetCoreAPIMakePC.Infrastructure.External;

    public class PaymentGatewayServiceTests
    {
        [Test]
        public async Task ProcessPaymentAsync_Returns_ExternalPaymentResponse()
        {
            var expectedResponse = new ExternalPaymentResponse { IsSuccess = true, Message = "OK" };
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(expectedResponse))
                });

            var httpClient = new HttpClient(handlerMock.Object);
            var service = new PaymentGatewayService(httpClient);

            var result = await service.ProcessPaymentAsync(100, "4111111111111111");

            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual("OK", result.Message);
        }
    }
}

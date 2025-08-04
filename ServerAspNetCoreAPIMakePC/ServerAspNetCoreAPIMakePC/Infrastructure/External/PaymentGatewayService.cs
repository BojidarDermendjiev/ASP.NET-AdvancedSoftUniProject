namespace ServerAspNetCoreAPIMakePC.Infrastructure.External
{
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;

    using Models;
    using Application.Interfaces;
    public class PaymentGatewayService : IPaymentGatewayService
    {
        private readonly HttpClient _httpClient;

        public PaymentGatewayService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ExternalPaymentResponse> ProcessPaymentAsync(decimal amount, string cardNumber)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/pay", new { amount, cardNumber });
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ExternalPaymentResponse>(json);
        }
    }
}

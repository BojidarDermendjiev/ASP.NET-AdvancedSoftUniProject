namespace ServerAspNetCoreAPIMakePC.Application.Interfaces
{
    using Infrastructure.Models;

    public interface IPaymentGatewayService
    {
        Task<ExternalPaymentResponse> ProcessPaymentAsync(decimal amount, string cardNumber);
    }
}

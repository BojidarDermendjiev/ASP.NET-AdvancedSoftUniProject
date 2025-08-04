namespace ServerAspNetCoreAPIMakePC.Infrastructure.Models
{
    public class ExternalPaymentResponse
    {
        public Guid TransactionId { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}

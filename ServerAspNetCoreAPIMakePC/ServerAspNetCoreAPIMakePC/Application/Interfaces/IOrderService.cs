namespace ServerAspNetCoreAPIMakePC.Application.Interfaces
{
    using DTOs.Order;

    public interface IOrderService
    {
        Task<OrderDto?> GetByIdAsync(int id);
        Task<IEnumerable<OrderDto>> GetByUserIdAsync(Guid userId);
        Task<IEnumerable<OrderDto>> GetAllAsync();
        Task<OrderDto> CreateAsync(CreateOrderDto dto);
        Task<OrderDto> UpdateAsync(UpdateOrderDto dto);
        Task DeleteAsync(int id);
    }
}

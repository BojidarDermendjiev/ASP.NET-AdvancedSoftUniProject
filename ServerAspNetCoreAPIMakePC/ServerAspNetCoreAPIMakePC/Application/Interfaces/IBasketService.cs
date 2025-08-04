namespace ServerAspNetCoreAPIMakePC.Application.Interfaces
{
    using DTOs.Basket;

    public interface IBasketService
    {
        Task<BasketDto?> GetByIdAsync(int id);
        Task<BasketDto?> GetByUserIdAsync(Guid userId);
        Task<IEnumerable<BasketDto>> GetAllAsync();
        Task<BasketDto> CreateAsync(CreateBasketDto dto);
        Task<BasketDto> UpdateAsync(UpdateBasketDto dto);
        Task ClearBasketAsync(Guid userId);

        Task DeleteAsync(int id);
    }
}

namespace ServerAspNetCoreAPIMakePC.Domain.Interfaces
{
    using Entities;

    public interface IBasketRepository
    {
        Task<Basket?> GetByIdAsync(int id);
        Task<Basket?> GetByUserIdAsync(Guid userId);
        Task<IEnumerable<Basket>> GetAllAsync();
        Task AddAsync(Basket basket);
        Task UpdateAsync(Basket basket);
        Task DeleteAsync(int id);
    }
}

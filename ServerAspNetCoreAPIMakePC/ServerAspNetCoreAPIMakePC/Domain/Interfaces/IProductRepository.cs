namespace ServerAspNetCoreAPIMakePC.Domain.Interfaces
{
    using Entities;

    public interface IProductRepository
    {
        Task<Product?> GetByIdAsync(Guid id);
        Task<Product?> GetByNameAsync(string name);
        Task<IEnumerable<Product>> GetAllAsync();
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<Product>> SearchAsync(string query);
    }
}

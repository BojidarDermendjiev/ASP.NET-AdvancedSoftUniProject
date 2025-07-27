namespace ServerAspNetCoreAPIMakePC.Application.Interfaces
{
    using DTOs;

    using Domain.Entities;

    public interface IProductService
    {
        Task<Product?> GetProductByIdAsync(Guid id);
        Task<Product> CreateProductAsync(CreateProductDto dto);
        Task<Product?> UpdateProductAsync(Guid id, UpdateProductDto dto);
        Task DeleteProductAsync(Guid id);
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<bool> ProductExistsAsync(string name);
    }
}

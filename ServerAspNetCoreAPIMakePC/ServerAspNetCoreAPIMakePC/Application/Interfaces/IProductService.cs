namespace ServerAspNetCoreAPIMakePC.Application.Interfaces
{
    using DTOs;

    using Domain.Entities;
    using ServerAspNetCoreAPIMakePC.Application.DTOs.Product;

    public interface IProductService
    {
        Task<Product?> GetProductByIdAsync(Guid id);
        Task<Product> CreateProductAsync(CreateProductDto dto);
        Task<Product?> UpdateProductAsync(Guid id, UpdateProductDto dto);
        Task DeleteProductAsync(Guid id);
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<bool> ProductExistsAsync(string name);
        Task<IEnumerable<Product>> SearchProductsAsync(string query);
        Task<(IEnumerable<Product> Products, int TotalCount)> GetProductsPagedAsync(int pageNumber, int pageSize);
    }
}

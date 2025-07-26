namespace ServerAspNetCoreAPIMakePC.Application.Interfaces
{
    using DTOs;

    using Domain.Entities;

    public interface IProductService
    {
        Task<Product?> GetProductByIdAsync(Guid id);
        Task<Product> CreateProductAsync(CreateProductDto dto);
    }
}

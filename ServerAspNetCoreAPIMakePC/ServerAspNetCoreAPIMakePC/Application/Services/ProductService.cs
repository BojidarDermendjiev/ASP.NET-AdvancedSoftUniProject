namespace ServerAspNetCoreAPIMakePC.Application.Services
{
    using AutoMapper;

    using DTOs;
    using Interfaces;
    using Domain.Entities;
    using Domain.Interfaces;


    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            this._productRepository = productRepository;
            this._mapper = mapper;
        }

        /// <summary>
        /// Get a product by its unique identifier.
        /// </summary>
        public Task<Product?> GetProductByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Create a new product from the given DTO.
        /// </summary>
        public Task<Product> CreateProductAsync(CreateProductDto dto)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Update an existing product.
        /// </summary>
        public Task<Product?> UpdateProductAsync(Guid id, UpdateProductDto dto)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Delete a product by its ID.
        /// </summary>
        public Task DeleteProductAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get all products.
        /// </summary>
        public Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Check if a product exists by name.
        /// </summary>
        public Task<bool> ProductExistsAsync(string name)
        {
            throw new NotImplementedException();
        }
    }
}

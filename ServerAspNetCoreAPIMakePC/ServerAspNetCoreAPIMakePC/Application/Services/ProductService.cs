namespace ServerAspNetCoreAPIMakePC.Application.Services
{
    using AutoMapper;

    using DTOs;
    using Interfaces;
    using Domain.Entities;
    using Domain.Interfaces;
    using static Domain.ErrorMessages.ErrorMessages;


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
        public async Task<Product?> GetProductByIdAsync(Guid id)
        {
            return await this._productRepository.GetByIdAsync(id);
        }

        /// <summary>
        /// Create a new product from the given DTO.
        /// </summary>
        public async Task<Product> CreateProductAsync(CreateProductDto dto)
        {
            var existing = await this._productRepository.GetByNameAsync(dto.Name);
            if (existing != null)
            {
                throw new InvalidOperationException(string.Format(AlreadyExistingProduct, dto.Name));
            }
            var product = this._mapper.Map<Product>(dto);
            product.Id = Guid.NewGuid();
            await this._productRepository.AddAsync(product);
            return product;

        }

        /// <summary>
        /// Update an existing product.
        /// </summary>
        public async Task<Product?> UpdateProductAsync(Guid id, UpdateProductDto dto)
        {
            var product = await this._productRepository.GetByIdAsync(id);
            if (product is null)
            {
                throw new KeyNotFoundException(string.Format(ProductNotFound));
            }

            if (!string.IsNullOrWhiteSpace(dto.Name) && dto.Name != product.Name)
            {
                var existing = await this._productRepository.GetByNameAsync(dto.Name);
                if (existing != null && existing.Id != id)
                {
                    throw new InvalidOperationException(string.Format(AlreadyExistingProduct, dto.Name));
                }
            }

            this._mapper.Map(dto, product);
            await this._productRepository.UpdateAsync(product);
            return product;
        }

        /// <summary>
        /// Delete a product by its ID.
        /// </summary>
        public async Task DeleteProductAsync(Guid id)
        {
            var product = await this._productRepository.GetByIdAsync(id);
            if (product is null)
            {
                throw new KeyNotFoundException(string.Format(ProductNotFound));
            }
            await this._productRepository.DeleteAsync(id);
        }

        /// <summary>
        /// Get all products.
        /// </summary>
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await this._productRepository.GetAllAsync();
        }

        /// <summary>
        /// Check if a product exists by name.
        /// </summary>
        public async Task<bool> ProductExistsAsync(string name)
        {
            var product = await this._productRepository.GetByNameAsync(name);
            return product != null;
        }

        /// <summary>
        /// Searches for products that match the specified query string.
        /// </summary>
        public async Task<IEnumerable<Product>> SearchProductsAsync(string query)
        {
            return await _productRepository.SearchAsync(query);
        }
    }
}

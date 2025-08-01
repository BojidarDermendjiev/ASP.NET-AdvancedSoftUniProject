namespace ServerAspNetCoreAPIMakePC.Application.Services
{
    using AutoMapper;

    using Interfaces;
    using DTOs.Product;
    using Domain.Entities;
    using Domain.Interfaces;
    using static Domain.ErrorMessages.ErrorMessages;

    /// <summary>
    /// Service class responsible for product management operations.
    /// Provides business logic for creating, retrieving, updating, deleting, and searching products,
    /// as well as checking for product existence and supporting pagination.
    /// Uses a product repository for data persistence and AutoMapper for DTO-entity mapping.
    /// </summary>
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductService"/> class.
        /// </summary>
        /// <param name="productRepository">Repository for product data operations.</param>
        /// <param name="mapper">AutoMapper instance for mapping between entities and DTOs.</param>
        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            this._productRepository = productRepository;
            this._mapper = mapper;
        }


        /// <summary>
        /// Retrieves a product by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the product.</param>
        /// <returns>The product entity if found; otherwise, null.</returns>
        public async Task<Product?> GetProductByIdAsync(Guid id)
        {
            return await this._productRepository.GetByIdAsync(id);
        }

        /// <summary>
        /// Creates a new product from the provided DTO.
        /// Throws an exception if a product with the same name already exists.
        /// </summary>
        /// <param name="dto">The data transfer object containing product creation data.</param>
        /// <returns>The created product entity.</returns>
        /// <exception cref="InvalidOperationException">Thrown if a product with the same name already exists.</exception>
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
        /// Updates an existing product with the provided data.
        /// Throws an exception if the product does not exist or if the new name is already in use.
        /// </summary>
        /// <param name="id">The unique identifier of the product to update.</param>
        /// <param name="dto">The data transfer object containing updated product data.</param>
        /// <returns>The updated product entity.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if the product does not exist.</exception>
        /// <exception cref="InvalidOperationException">Thrown if a product with the new name already exists.</exception>
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
        /// Deletes a product by its unique identifier.
        /// Throws an exception if the product does not exist.
        /// </summary>
        /// <param name="id">The unique identifier of the product to delete.</param>
        /// <exception cref="KeyNotFoundException">Thrown if the product does not exist.</exception>
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
        /// Retrieves all products.
        /// </summary>
        /// <returns>A collection of all product entities.</returns>
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await this._productRepository.GetAllAsync();
        }

        /// <summary>
        /// Checks if a product exists by name.
        /// </summary>
        /// <param name="name">The product name to check for existence.</param>
        /// <returns>True if a product with the specified name exists; otherwise, false.</returns>
        public async Task<bool> ProductExistsAsync(string name)
        {
            var product = await this._productRepository.GetByNameAsync(name);
            return product != null;
        }

        /// <summary>
        /// Searches for products that match the specified query string.
        /// </summary>
        /// <param name="query">The search query string.</param>
        /// <returns>A collection of products matching the search criteria.</returns>
        public async Task<IEnumerable<Product>> SearchProductsAsync(string query)
        {
            return await _productRepository.SearchAsync(query);
        }


        /// <summary>
        /// Retrieves a paginated list of products.
        /// </summary>
        /// <param name="pageNumber">The zero-based page number.</param>
        /// <param name="pageSize">The size of the page.</param>
        /// <returns>A tuple containing the paginated products and the total count.</returns>
        public async Task<(IEnumerable<Product> Products, int TotalCount)> GetProductsPagedAsync(int pageNumber, int pageSize)
        {
            return await this._productRepository.GetPagesAsync(pageNumber, pageSize);
        }
    }
}

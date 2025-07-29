namespace ServerAspNetCoreAPIMakePC.Infrastructure.Repositories
{
    using Microsoft.EntityFrameworkCore;

    using Data;
    using Domain.Entities;
    using Domain.Interfaces;

    /// <summary>
    /// Repository implementation for managing Product entities in the database.
    /// Provides CRUD operations, searching, and pagination for products.
    /// </summary>
    public class ProductRepository : IProductRepository
    {
        private readonly MakePCDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductRepository"/> class.
        /// </summary>
        public ProductRepository(MakePCDbContext context)
        {
            this._context = context;
        }

        /// <summary>
        /// Retrieves a product by its unique identifier.
        /// </summary>
        public async Task<Product?> GetByIdAsync(Guid id)
        {
            return await this._context.Products
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        /// <summary>
        /// Retrieves a product by its name.
        /// </summary>
        public async Task<Product?> GetByNameAsync(string name)
        {

            return await this._context.Products
                .FirstOrDefaultAsync(p => p.Name == name);
        }

        /// <summary>
        /// Retrieves all products from the database.
        /// </summary>
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await this._context.Products.ToListAsync();
        }

        /// <summary>
        /// Adds a new product to the database.
        /// </summary>
        public async Task AddAsync(Product product)
        {
            await this._context.Products.AddAsync(product);
            await this._context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates an existing product in the database.
        /// </summary>
        public async Task UpdateAsync(Product product)
        {
            this._context.Products.Update(product);
            await this._context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a product by its unique identifier.
        /// </summary>

        public async Task DeleteAsync(Guid id)
        {

            var product = await this._context.Products
                .FindAsync(id);
            if (product != null)
            {
                this._context.Products.Remove(product);
                await this._context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Searches for products by name or description containing the specified query string.
        /// </summary>
        public async Task<IEnumerable<Product>> SearchAsync(string query)
        {
            return await this._context.Products
                .Where(p => p.Name.Contains(query) || p.Description.Contains(query))
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves a paginated list of products and the total count of products in the database.
        /// </summary>
        public async Task<(IEnumerable<Product> products, int totalCount)> GetPagesAsync(int pageNumber, int pageSize)
        {
            var query = _context.Products.AsQueryable();
            var totalCount = await query.CountAsync();
            var products = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (products, totalCount);
        }
    }
}

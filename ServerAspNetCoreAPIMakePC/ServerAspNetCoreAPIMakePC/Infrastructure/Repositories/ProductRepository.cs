namespace ServerAspNetCoreAPIMakePC.Infrastructure.Repositories
{
    using Microsoft.EntityFrameworkCore;

    using Data;
    using Domain.Entities;
    using Domain.Interfaces;

    public class ProductRepository : IProductRepository
    {
        private readonly MakePCDbContext _context;

        public ProductRepository(MakePCDbContext context)
            => this._context = context;
        public async Task<Product?> GetByIdAsync(Guid id)
        {
            return await this._context.Products
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Product?> GetByNameAsync(string name)
        {

            return await this._context.Products
                .FirstOrDefaultAsync(p => p.Name == name);
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await this._context.Products.ToListAsync();
        }

        public async Task AddAsync(Product product)
        {
            await this._context.Products.AddAsync(product);
            await this._context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            this._context.Products.Update(product);
            await this._context.SaveChangesAsync();
        }

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

        public async Task<IEnumerable<Product>> SearchAsync(string query)
        {
            return await this._context.Products
                .Where(p => p.Name.Contains(query) || p.Description.Contains(query))
                .ToListAsync();
        }
    }
}

namespace ServerAspNetCoreAPIMakePC.Infrastructure.Repositories
{
    using Microsoft.EntityFrameworkCore;
    
    using Data;
    using Domain.Entities;
    using Domain.Interfaces;
    using static Domain.ErrorMessages.ErrorMessages;

    public class BrandRepository : IBrandRepository
    {
        private readonly MakePCDbContext _context;

        public BrandRepository(MakePCDbContext context)
        {
            this._context = context;
        }
        public async Task<Brand?> GetByIdAsync(int id)
        {
            return await this._context.Brands.FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Brand>> GetAllAsync()
        {
            return await this._context.Brands.ToListAsync();
        }

        public async Task AddAsync(Brand brand)
        {
            if (brand  is null)
            {
                throw new ArgumentNullException(nameof(brand), string.Format(InvalidBrandCannotBeNull));
            }
            this._context.Add(brand);
            await this._context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Brand brand)
        {
            if (brand is null)
            {
                throw new ArgumentNullException(nameof(brand), string.Format(InvalidBrandCannotBeNull));
            }
            this._context.Update(brand);
            await this._context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var brand = await this._context.Brands.FindAsync(id);
            if (brand is null)
            {
                throw new ArgumentNullException(nameof(brand), string.Format(InvalidBrandCannotBeNull));
            }

            this._context.Brands.Remove(brand);
            await this._context.SaveChangesAsync();
        }
    }
}

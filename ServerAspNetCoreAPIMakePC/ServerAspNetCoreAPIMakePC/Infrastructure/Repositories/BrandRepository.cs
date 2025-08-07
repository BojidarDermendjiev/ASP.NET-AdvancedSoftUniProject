namespace ServerAspNetCoreAPIMakePC.Infrastructure.Repositories
{
    using Microsoft.EntityFrameworkCore;

    using Data;
    using Domain.Entities;
    using Domain.Interfaces;
    using static Domain.ErrorMessages.ErrorMessages;

    /// <summary>
    /// Repository implementation for managing Brand entities in the database.
    /// Provides methods for CRUD operations and retrieval of brands.
    /// </summary>
    public class BrandRepository : IBrandRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BrandRepository"/> class.
        /// </summary>
        /// <param name="context">The database context for performing brand operations.</param>
        /// 
        private readonly MakePCDbContext _context;

        public BrandRepository(MakePCDbContext context)
        {
            this._context = context;
        }

        /// <summary>
        /// Retrieves a brand by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the brand.</param>
        /// <returns>The brand entity if found; otherwise, null.</returns>
        public async Task<Brand?> GetByIdAsync(int id)
        {
            return await this._context.Brands.FirstOrDefaultAsync(b => b.Id == id);
        }

        /// <summary>
        /// Retrieves all brands from the database.
        /// </summary>
        /// <returns>A collection of brand entities.</returns>
        public async Task<IEnumerable<Brand>> GetAllAsync()
        {
            return await this._context.Brands.ToListAsync();
        }

        /// <summary>
        /// Adds a new brand entity to the database.
        /// </summary>
        /// <param name="brand">The brand entity to add.</param>
        /// <exception cref="ArgumentNullException">Thrown if the brand is null.</exception>
        public async Task AddAsync(Brand brand)
        {
            if (brand is null)
            {
                throw new ArgumentNullException(nameof(brand), string.Format(InvalidBrandCannotBeNull));
            }
            this._context.Add(brand);
            await this._context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates an existing brand entity in the database.
        /// </summary>
        /// <param name="brand">The brand entity with updated values.</param>
        /// <exception cref="ArgumentNullException">Thrown if the brand is null.</exception>
        public async Task UpdateAsync(Brand brand)
        {
            if (brand is null)
            {
                throw new ArgumentNullException(nameof(brand), string.Format(InvalidBrandCannotBeNull));
            }
            this._context.Update(brand);
            await this._context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a brand entity from the database by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the brand to delete.</param>
        /// <exception cref="ArgumentNullException">Thrown if the brand is not found.</exception>
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

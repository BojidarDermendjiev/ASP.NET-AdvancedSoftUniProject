namespace ServerAspNetCoreAPIMakePC.Infrastructure.Repositories
{
    using Microsoft.EntityFrameworkCore;

    using Data;
    using Domain.Entities;
    using Domain.Interfaces;
    using static Domain.ErrorMessages.ErrorMessages;

    /// <summary>
    /// Repository implementation for managing Category entities in the database.
    /// Provides methods for CRUD (Create, Read, Update, Delete) operations on categories.
    /// </summary>
    public class CategoryRepository : ICategoryRepository
    {
        private readonly MakePCDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryRepository"/> class.
        /// </summary>
        public CategoryRepository(MakePCDbContext context)
        {
            this._context = context;
        }

        /// <summary>
        /// Retrieves a category by its unique identifier.
        /// </summary>
        public async Task<Category?> GetByIdAsync(int id)
        {
            return await this._context.Categories
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        /// <summary>
        /// Retrieves all categories from the database.
        /// </summary>
        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await this._context.Categories
                .ToListAsync();
        }

        /// <summary>
        /// Adds a new category to the database.
        /// </summary>
        public async Task AddAsync(Category category)
        {
            if (category is null)
            {
                throw new ArgumentNullException(nameof(category), string.Format(InvalidCategoryCannotBeNull));
            }
            await this._context.Categories.AddAsync(category);
            await this._context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates an existing category in the database.
        /// </summary>
        public async Task UpdateAsync(Category category)
        {
            if (category is null)
            {
                throw new ArgumentNullException(nameof(category), string.Format(InvalidCategoryCannotBeNull));
            }

            this._context.Update(category);
            await this._context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a category by its unique identifier.
        /// </summary>
        public async Task DeleteAsync(int id)
        {
            var category = await this._context.Categories.FindAsync(id);
            if (category is null)
            {
                throw new KeyNotFoundException(string.Format(InvalidCategoryCannotBeNull));
            }
            this._context.Categories.Remove(category);
            await this._context.SaveChangesAsync();
        }
    }
}

namespace ServerAspNetCoreAPIMakePC.Infrastructure.Repositories
{
    using Microsoft.EntityFrameworkCore;

    using Data;
    using Domain.Entities;
    using Domain.Interfaces;
    using static Domain.ErrorMessages.ErrorMessages;

    /// <summary>
    /// Repository implementation for managing Basket entities in the database.
    /// Provides methods for CRUD operations and retrieval of baskets, including eager loading of related items, products, and user data.
    /// </summary>
    public class BasketRepository : IBasketRepository
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="BasketRepository"/> class.
        /// </summary>
        /// <param name="context">The database context for performing basket operations.</param>

        private readonly MakePCDbContext _context;

        public BasketRepository(MakePCDbContext context)
        {
            this._context = context;
        }

        /// <summary>
        /// Retrieves a basket by its unique identifier, including its items, products, and user.
        /// </summary>
        /// <param name="id">The unique identifier of the basket.</param>
        /// <returns>The basket entity if found; otherwise, null.</returns>
        public async Task<Basket?> GetByIdAsync(int id)
        {
            return await this._context.Baskets
                .Include(b => b.Items)
                .ThenInclude(i => i.Product)
                .Include(b => b.User)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        /// <summary>
        /// Retrieves a basket associated with a specific user, including its items, products, and user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>The basket entity if found; otherwise, null.</returns>
        public async Task<Basket?> GetByUserIdAsync(Guid userId)
        {
            return await this._context.Baskets
                .Include(b => b.Items)
                .ThenInclude(i => i.Product)
                .Include(b => b.User)
                .FirstOrDefaultAsync(b => b.UserId == userId);
        }

        /// <summary>
        /// Retrieves all baskets from the database, including their items, products, and user data.
        /// </summary>
        /// <returns>A collection of basket entities.</returns>
        public async Task<IEnumerable<Basket>> GetAllAsync()
        {
            return await this._context.Baskets
                .Include(b => b.Items)
                .ThenInclude(i => i.Product)
                .Include(b => b.User)
                .ToListAsync();
        }

        /// <summary>
        /// Adds a new basket entity to the database.
        /// </summary>
        /// <param name="basket">The basket entity to add.</param>
        /// <exception cref="ArgumentNullException">Thrown if the basket is null.</exception>
        public async Task AddAsync(Basket basket)
        {
            if (basket is null)
            {
                throw new ArgumentNullException(nameof(basket), string.Format(InvalidBasketCannotBeNull));
            }
            await this._context.Baskets.AddAsync(basket);
            await this._context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates an existing basket entity in the database.
        /// </summary>
        /// <param name="basket">The basket entity with updated values.</param>
        /// <exception cref="ArgumentNullException">Thrown if the basket is null.</exception>
        /// <exception cref="KeyNotFoundException">Thrown if the basket does not exist.</exception>
        public async Task UpdateAsync(Basket basket)
        {
            if (basket is null)
            {
                throw new ArgumentNullException(nameof(basket), string.Format(InvalidBasketCannotBeNull));
            }
            var existingBasket = await this.GetByIdAsync(basket.Id);
            if (existingBasket is null)
            {
                throw new KeyNotFoundException(string.Format(InvalidBasketNotFound));
            }
            this._context.Baskets.Update(basket);
            await this._context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a basket entity from the database by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the basket to delete.</param>
        /// <exception cref="KeyNotFoundException">Thrown if the basket does not exist.</exception>
        public async Task DeleteAsync(int id)
        {
            var basket = await this.GetByIdAsync(id);
            if (basket is null)
            {
                throw new KeyNotFoundException(string.Format(InvalidBasketNotFound));
            }
            this._context.Baskets.Remove(basket);
            await this._context.SaveChangesAsync();
        }
    }
}

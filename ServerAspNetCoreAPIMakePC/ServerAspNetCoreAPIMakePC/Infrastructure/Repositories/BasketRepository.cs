namespace ServerAspNetCoreAPIMakePC.Infrastructure.Repositories
{
    using Microsoft.EntityFrameworkCore;
    
    using Data;
    using Domain.Entities;
    using Domain.Interfaces;
    using static Domain.ErrorMessages.ErrorMessages;
    public class BasketRepository : IBasketRepository
    {
        private readonly MakePCDbContext _context;

        public BasketRepository(MakePCDbContext context)
        {
            this._context = context;
        }
        public async Task<Basket?> GetByIdAsync(int id)
        {
            return await this._context.Baskets
                .Include(b => b.Items)
                .ThenInclude(i => i.Product)
                .Include(b => b.User)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Basket?> GetByUserIdAsync(Guid userId)
        {
            return await this._context.Baskets
                .Include(b => b.Items)
                .ThenInclude(i => i.Product)
                .Include(b => b.User)
                .FirstOrDefaultAsync(b => b.UserId == userId);
        }

        public async Task<IEnumerable<Basket>> GetAllAsync()
        {
            return await this._context.Baskets
                .Include(b => b.Items)
                .ThenInclude(i => i.Product)
                .Include(b => b.User)
                .ToListAsync();
        }

        public async Task AddAsync(Basket basket)
        {
            if (basket is null)
            {
                throw new ArgumentNullException(nameof(basket), string.Format(InvalidBasketCannotBeNull));
            }
            await this._context.Baskets.AddAsync(basket);
            await this._context.SaveChangesAsync();
        }

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

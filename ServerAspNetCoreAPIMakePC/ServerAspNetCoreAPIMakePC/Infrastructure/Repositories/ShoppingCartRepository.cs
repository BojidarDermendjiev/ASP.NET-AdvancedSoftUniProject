namespace ServerAspNetCoreAPIMakePC.Infrastructure.Repositories
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;

    using Data;
    using Domain.Entities;
    using Domain.Interfaces;

    /// <summary>
    /// Repository for CRUD operations on ShoppingCart entities, including upserts and deletion by user.
    /// </summary>
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly MakePCDbContext _context;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShoppingCartRepository"/> class.
        /// </summary>
        public ShoppingCartRepository(MakePCDbContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        /// <summary>
        /// Retrieves the shopping cart for a specific user, including its items.
        /// </summary>
        public async Task<ShoppingCart?> GetByUserIdAsync(Guid userId)
        {
            return await this._context.ShoppingCarts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        /// <summary>
        /// Inserts or updates a shopping cart in the database. 
        /// If a cart for the user already exists, updates its items and creation date; otherwise, adds a new cart.
        /// </summary>
        public async Task UpsertAsync(ShoppingCart cart)
        {
            var existingCart = await this._context.ShoppingCarts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == cart.UserId);

            if (existingCart is null)
            {
                await this._context.ShoppingCarts.AddAsync(cart);
            }
            else
            {
                foreach (var item in existingCart.Items.ToList())
                {
                    if (cart.Items.All(i => i.Id != item.Id))
                    {
                        this._context.Set<BasketItem>().Remove(item);
                    }
                }

                foreach (var basketItem in cart.Items)
                {
                    var existingItem = existingCart.Items.FirstOrDefault(i => i.Id == basketItem.Id);
                    if (existingItem is null)
                    {
                        existingCart.Items.Add(basketItem);
                    }
                    else
                    {
                        this._mapper.Map(basketItem, existingItem);
                    }
                }

                existingCart.DateCreated = cart.DateCreated;
            }

            await this._context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes the shopping cart (and its items) for a specific user.
        /// </summary>
        public async Task DeleteByUserIdAsync(Guid userId)
        {
            var cart = await this._context.ShoppingCarts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart != null)
            {
                this._context.ShoppingCarts.Remove(cart);
                await this._context.SaveChangesAsync();
            }
        }
    }
}

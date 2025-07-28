namespace ServerAspNetCoreAPIMakePC.Infrastructure.Repositories
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;

    using Data;
    using Domain.Entities;
    using Domain.Interfaces;

    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly MakePCDbContext _context;
        private readonly IMapper _mapper;

        public ShoppingCartRepository(MakePCDbContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<ShoppingCart?> GetByUserIdAsync(Guid userId)
        {
            return await _context.ShoppingCarts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task UpsertAsync(ShoppingCart cart)
        {
            var existingCart = await _context.ShoppingCarts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == cart.UserId);

            if (existingCart is null)
            {
                await _context.ShoppingCarts.AddAsync(cart);
            }
            else
            {
                foreach (var item in existingCart.Items.ToList())
                {
                    if (cart.Items.All(i => i.Id != item.Id))
                    {
                        _context.Set<BasketItem>().Remove(item);
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
                        _mapper.Map(basketItem, existingItem);
                    }
                }

                existingCart.DateCreated = cart.DateCreated;
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteByUserIdAsync(Guid userId)
        {
            var cart = await _context.ShoppingCarts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart != null)
            {
                _context.ShoppingCarts.Remove(cart);
                await _context.SaveChangesAsync();
            }
        }
    }
}

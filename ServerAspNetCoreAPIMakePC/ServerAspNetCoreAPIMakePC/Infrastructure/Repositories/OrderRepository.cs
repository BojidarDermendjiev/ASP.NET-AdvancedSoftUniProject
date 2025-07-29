namespace ServerAspNetCoreAPIMakePC.Infrastructure.Repositories
{
    using Microsoft.EntityFrameworkCore;

    using Data;
    using Domain.Entities;
    using Domain.Interfaces;
    using static Domain.ErrorMessages.ErrorMessages;

    /// <summary>
    /// Repository implementation for managing Order entities in the database.
    /// Provides methods for CRUD operations and retrieval of orders, including eager loading of related data.
    /// </summary>
    public class OrderRepository : IOrderRepository
    {
        private readonly MakePCDbContext _context;


        /// <summary>
        /// Initializes a new instance of the <see cref="OrderRepository"/> class.
        /// </summary>
        /// <param name="context">The database context for performing order operations.</param>
        public OrderRepository(MakePCDbContext context)
        {
            this._context = context;
        }

        /// <summary>
        /// Retrieves an order by its unique identifier, including its items, products, and user.
        /// </summary>
        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.Items)
                .ThenInclude(oi => oi.Product)
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        /// <summary>
        /// Retrieves all orders for a specific user, including their items, products, and user details.
        /// </summary>
        public async Task<IEnumerable<Order>> GetByUserIdAsync(Guid userId)
        {
            return await _context.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .Include(o => o.User)
                .Where(o => o.UserId == userId)
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves all orders from the database, including their items, products, and user details.
        /// </summary>
        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _context.Orders
                .Include(o => o.Items)
                .ThenInclude(oi => oi.Product)
                .Include(o => o.User)
                .ToListAsync();
        }

        /// <summary>
        /// Adds a new order to the database.
        /// </summary>
        public async Task AddAsync(Order order)
        {
            if (order is null)
            {
                throw new ArgumentNullException(nameof(order), string.Format(InvalidOrder));
            }
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates an existing order in the database.
        /// </summary>
        public async Task UpdateAsync(Order order)
        {
            if (order is null)
            {
                throw new ArgumentNullException(nameof(order), string.Format(InvalidOrder));
            }
            var existingOrder = await GetByIdAsync(order.Id);
            if (existingOrder is null)
            {
                throw new KeyNotFoundException(OrderNotFound);
            }
            _context.Entry(existingOrder).CurrentValues.SetValues(order);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes an order by its unique identifier.
        /// </summary>
        public async Task DeleteAsync(int id)
        {
            var order = await GetByIdAsync(id);
            if (order is null)
            {
                throw new KeyNotFoundException(OrderNotFound);
            }
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }
    }
}

namespace ServerAspNetCoreAPIMakePC.Domain.Interfaces
{
    using Entities;

    public interface IShoppingCartRepository
    {
        // <summary>
        /// Gets the shopping cart for a given user by their user ID.
        /// </summary>
        Task<ShoppingCart?> GetByUserIdAsync(Guid userId);

        /// <summary>
        /// Creates or updates the shopping cart in the data store.
        /// </summary>
        Task UpsertAsync(ShoppingCart cart);

        /// <summary>
        /// Deletes the shopping cart for a given user.
        /// </summary>
        Task DeleteByUserIdAsync(Guid userId);
    }
}

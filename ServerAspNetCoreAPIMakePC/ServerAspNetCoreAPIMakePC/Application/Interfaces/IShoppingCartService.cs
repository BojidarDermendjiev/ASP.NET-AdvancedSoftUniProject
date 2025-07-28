namespace ServerAspNetCoreAPIMakePC.Application.Interfaces
{
    using DTOs.ShoppingCart;

    public interface IShoppingCartService
    {
        Task<ShoppingCartDto?> GetCartByUserIdAsync(Guid userId);
        Task<ShoppingCartDto> AddItemAsync(AddBasketItemDto dto);
        Task<ShoppingCartDto> RemoveItemAsync(RemoveBasketItemDto dto);
        Task ClearCartAsync(Guid userId);
    }
}

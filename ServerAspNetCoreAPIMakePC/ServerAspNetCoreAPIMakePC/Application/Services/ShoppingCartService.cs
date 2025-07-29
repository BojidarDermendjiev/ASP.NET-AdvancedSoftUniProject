namespace ServerAspNetCoreAPIMakePC.Application.Services
{
    using AutoMapper;

    using Interfaces;
    using Domain.Entities;
    using Domain.Interfaces;
    using DTOs.ShoppingCart;
    using static Domain.ErrorMessages.ErrorMessages;
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ShoppingCartService(IShoppingCartRepository shoppingCartRepository, IProductRepository productRepository, IMapper mapper)
        {
            this._shoppingCartRepository = shoppingCartRepository;
            this._productRepository = productRepository;
            this._mapper = mapper;
        }

        /// <summary>
        /// Retrieves the shopping cart for the specified user.
        /// </summary>
        public async Task<ShoppingCartDto?> GetCartByUserIdAsync(Guid userId)
        {
            var cart = await this._shoppingCartRepository.GetByUserIdAsync(userId);
            return cart is null ? null : this._mapper.Map<ShoppingCartDto>(cart);
        }

        /// <summary>
        /// Adds an item to the user's shopping cart. 
        /// If the cart does not exist, a new cart is created. 
        /// If the item already exists in the cart, its quantity is increased.
        /// </summary>
        public async Task<ShoppingCartDto> AddItemAsync(AddBasketItemDto dto)
        {
            var cart = await this._shoppingCartRepository.GetByUserIdAsync(dto.UserId);
            if (cart is null)
            {
                cart = _mapper.Map<ShoppingCart>(dto);
            }

            var product = await this._productRepository.GetByIdAsync(dto.ProductId);
            if (product is null)
            {
                throw new ArgumentException(string.Format(ProductNotExist));
            }
            var basketItem = cart.Items.FirstOrDefault(i => i.ProductId == dto.ProductId);
            if (basketItem is null)
            {
                basketItem = _mapper.Map<BasketItem>(dto);
            }
            else
            {
                basketItem.Quantity += dto.Quantity;
                cart.Items.Add(basketItem);
            }
            await this._shoppingCartRepository.UpsertAsync(cart);
            return _mapper.Map<ShoppingCartDto>(cart);
        }

        /// <summary>
        /// Removes an item from the user's shopping cart.
        /// </summary>
        public async Task<ShoppingCartDto> RemoveItemAsync(RemoveBasketItemDto dto)
        {
            var cart = await this._shoppingCartRepository.GetByUserIdAsync(dto.UserId);
            if (cart is null)
            {
                throw new KeyNotFoundException(string.Format(ShoppingNotFound));
            }
            var basketItem = cart.Items.FirstOrDefault(i => i.ProductId == dto.ProductId);
            if (basketItem is null)
            {
                throw new KeyNotFoundException(string.Format(ProductNotFound));
            }
            cart.Items.Remove(basketItem);
            await this._shoppingCartRepository.UpsertAsync(cart);
            return _mapper.Map<ShoppingCartDto>(cart);
        }

        /// <summary>
        /// Removes all items from the user's shopping cart.
        /// </summary>
        public async Task ClearCartAsync(Guid userId)
        {
            var cart = await this._shoppingCartRepository.GetByUserIdAsync(userId);
            if (cart is null)
            {
                throw new KeyNotFoundException(string.Format(ShoppingNotFound));
            }
            cart.Items.Clear();
            await this._shoppingCartRepository.UpsertAsync(cart);
        }
    }
}

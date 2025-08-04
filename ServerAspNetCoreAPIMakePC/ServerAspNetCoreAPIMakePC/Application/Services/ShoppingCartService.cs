﻿namespace ServerAspNetCoreAPIMakePC.Application.Services
{
    using AutoMapper;
   
    using Interfaces;
    using Domain.Entities;
    using Domain.Interfaces;
    using DTOs.ShoppingCart;
    using Domain.ValueObjects;
    using static Domain.ErrorMessages.ErrorMessages;

    /// <summary>
    /// Service class responsible for managing shopping cart operations.
    /// Provides logic for retrieving, adding, removing, and clearing items in a user's shopping cart.
    /// Utilizes repositories for persistence and AutoMapper for DTO-entity mapping.
    /// </summary>
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShoppingCartService"/> class.
        /// </summary>
        /// <param name="shoppingCartRepository">Repository for shopping cart data operations.</param>
        /// <param name="productRepository">Repository for product data operations.</param>
        /// <param name="mapper">AutoMapper instance for mapping between entities and DTOs.</param>
        public ShoppingCartService(IShoppingCartRepository shoppingCartRepository, IProductRepository productRepository, IMapper mapper)
        {
            this._shoppingCartRepository = shoppingCartRepository;
            this._productRepository = productRepository;
            this._mapper = mapper;
        }

        /// <summary>
        /// Retrieves the shopping cart for the specified user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>The shopping cart as a <see cref="ShoppingCartDto"/>, or null if not found.</returns>
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
        /// <param name="dto">The item to add to the cart.</param>
        /// <returns>The updated shopping cart as a <see cref="ShoppingCartDto"/>.</returns>
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
                basketItem.Quantity = new Quantity(basketItem.Quantity.Value + dto.Quantity);
                cart.Items.Add(basketItem);
            }
            await this._shoppingCartRepository.UpsertAsync(cart);
            return _mapper.Map<ShoppingCartDto>(cart);
        }

        /// <summary>
        /// Removes an item from the user's shopping cart.
        /// </summary>
        /// <param name="dto">The item to remove from the cart.</param>
        /// <returns>The updated shopping cart as a <see cref="ShoppingCartDto"/>.</returns>
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
        /// <param name="userId">The unique identifier of the user whose cart will be cleared.</param>
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
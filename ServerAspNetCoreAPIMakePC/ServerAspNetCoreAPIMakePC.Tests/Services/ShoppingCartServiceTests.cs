namespace ServerAspNetCoreAPIMakePC.Tests.Services
{
    using Moq;
    using AutoMapper;
    using NUnit.Framework;
    using Microsoft.Extensions.Options;

    using Domain.Enums;
    using Domain.Entities;
    using Domain.Interfaces;
    using Domain.ValueObjects;
    using Application.DTOs.ShoppingCart;
    using ServerAspNetCoreAPIMakePC.Application.Services;
    using ServerAspNetCoreAPIMakePC.Application.DTOs.ShoppingCart;

    [TestFixture]
    public class ShoppingCartServiceTests
    {
        private Mock<IShoppingCartRepository> _shoppingCartRepositoryMock;
        private Mock<IProductRepository> _productRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private ShoppingCartService _shoppingCartService;

        [SetUp]
        public void SetUp()
        {
            this._shoppingCartRepositoryMock = new Mock<IShoppingCartRepository>();
            this._productRepositoryMock = new Mock<IProductRepository>();
            this._mapperMock = new Mock<IMapper>();

            this._shoppingCartService = new ShoppingCartService(
                this._shoppingCartRepositoryMock.Object,
                this._productRepositoryMock.Object,
                this._mapperMock.Object);
        }

        [Test]
        public async Task GetCartByUserIdAsync_Returns_CartDto_When_Cart_Exists()
        {
            var userId = Guid.NewGuid();
            var cart = new ShoppingCart { UserId = userId, Items = new List<BasketItem>() };
            var cartDto = new ShoppingCartDto();

            this._shoppingCartRepositoryMock.Setup(r => r.GetByUserIdAsync(userId)).ReturnsAsync(cart);
            this._mapperMock.Setup(m => m.Map<ShoppingCartDto>(cart)).Returns(cartDto);

            var result = await _shoppingCartService.GetCartByUserIdAsync(userId);

            Assert.IsNotNull(result);
            Assert.AreSame(cartDto, result);
        }

        [Test]
        public async Task GetCartByUserIdAsync_Returns_Null_When_Cart_Not_Exists()
        {
            var userId = Guid.NewGuid();
            this._shoppingCartRepositoryMock.Setup(r => r.GetByUserIdAsync(userId)).ReturnsAsync((ShoppingCart)null);

            var result = await this._shoppingCartService.GetCartByUserIdAsync(userId);

            Assert.IsNull(result);
        }

        [Test]
        public void AddItemAsync_Throws_When_Product_Does_Not_Exist()
        {
            var dto = new AddBasketItemDto { UserId = Guid.NewGuid(), ProductId = Guid.NewGuid(), Quantity = 1 };
            this._shoppingCartRepositoryMock.Setup(r => r.GetByUserIdAsync(dto.UserId)).ReturnsAsync((ShoppingCart)null);
            this._productRepositoryMock.Setup(r => r.GetByIdAsync(dto.ProductId)).ReturnsAsync((Product)null);

            Assert.ThrowsAsync<ArgumentException>(async () => await this._shoppingCartService.AddItemAsync(dto));
        }

        [Test]
        public async Task RemoveItemAsync_Throws_When_Cart_Not_Exists()
        {
            var dto = new RemoveBasketItemDto { UserId = Guid.NewGuid(), ProductId = Guid.NewGuid() };
            this._shoppingCartRepositoryMock.Setup(r => r.GetByUserIdAsync(dto.UserId)).ReturnsAsync((ShoppingCart)null);

            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await this._shoppingCartService.RemoveItemAsync(dto));
            Assert.That(ex.Message, Does.Contain("not found"));
        }

        [Test]
        public async Task RemoveItemAsync_Throws_When_Product_Not_Exists_In_Cart()
        {
            var dto = new RemoveBasketItemDto { UserId = Guid.NewGuid(), ProductId = Guid.NewGuid() };
            var cart = new ShoppingCart { UserId = dto.UserId, Items = new List<BasketItem>() };
            this._shoppingCartRepositoryMock.Setup(r => r.GetByUserIdAsync(dto.UserId)).ReturnsAsync(cart);

            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await this._shoppingCartService.RemoveItemAsync(dto));
            Assert.That(ex.Message, Does.Contain("not found"));
        }

        [Test]
        public async Task ClearCartAsync_Removes_All_Items()
        {
            var userId = Guid.NewGuid();
            var cart = new ShoppingCart
            {
                UserId = userId,
                Items = new List<BasketItem>
                {
                    new BasketItem { ProductId = Guid.NewGuid(), Quantity = new Quantity(2) }
                }
            };

            this._shoppingCartRepositoryMock.Setup(r => r.GetByUserIdAsync(userId)).ReturnsAsync(cart);

            await this._shoppingCartService.ClearCartAsync(userId);

            Assert.AreEqual(0, cart.Items.Count);
            this._shoppingCartRepositoryMock.Verify(r => r.UpsertAsync(cart), Times.Once);
        }
    }
}
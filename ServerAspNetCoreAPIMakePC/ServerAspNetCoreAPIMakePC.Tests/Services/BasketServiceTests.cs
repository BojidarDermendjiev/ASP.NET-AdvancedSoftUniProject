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
    using Application.Services;
    using Application.Settings;
    using Application.Interfaces;
    using Application.DTOs.Basket;

    [TestFixture]
    public class BasketServiceTests
    {
        private Mock<IBasketRepository> _basketRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private BasketService _basketService;

        [SetUp]
        public void SetUp()
        {
            _basketRepositoryMock = new Mock<IBasketRepository>();
            _mapperMock = new Mock<IMapper>();
            _basketService = new BasketService(_basketRepositoryMock.Object, _mapperMock.Object);
        }

        [Test]
        public async Task GetByIdAsync_ReturnsBasketDto_IfFound()
        {
            var basket = new Basket { Id = 1 };
            var basketDto = new BasketDto { Id = 1 };

            _basketRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(basket);
            _mapperMock.Setup(m => m.Map<BasketDto>(basket)).Returns(basketDto);

            var result = await _basketService.GetByIdAsync(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
        }

        [Test]
        public async Task GetByIdAsync_ReturnsNull_IfNotFound()
        {
            _basketRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Basket)null);

            var result = await _basketService.GetByIdAsync(1);

            Assert.IsNull(result);
        }

        [Test]
        public async Task GetAllAsync_ReturnsAllBasketDtos()
        {
            var baskets = new List<Basket> { new Basket { Id = 1 }, new Basket { Id = 2 } };
            var basketDtos = new List<BasketDto> { new BasketDto { Id = 1 }, new BasketDto { Id = 2 } };

            _basketRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(baskets);
            _mapperMock.Setup(m => m.Map<BasketDto>(baskets[0])).Returns(basketDtos[0]);
            _mapperMock.Setup(m => m.Map<BasketDto>(baskets[1])).Returns(basketDtos[1]);

            var result = (await _basketService.GetAllAsync());

            CollectionAssert.AreEquivalent(new[] { 1, 2 }, new List<BasketDto>(result).ConvertAll(d => d.Id));
        }

        [Test]
        public async Task CreateAsync_CreatesAndReturnsBasketDto()
        {
            var createDto = new CreateBasketDto();
            var basket = new Basket();
            var basketDto = new BasketDto();

            _mapperMock.Setup(m => m.Map<Basket>(createDto)).Returns(basket);
            _basketRepositoryMock.Setup(r => r.AddAsync(basket)).Returns(Task.CompletedTask);
            _mapperMock.Setup(m => m.Map<BasketDto>(basket)).Returns(basketDto);

            var result = await _basketService.CreateAsync(createDto);

            Assert.IsNotNull(result);
            _basketRepositoryMock.Verify(r => r.AddAsync(basket), Times.Once);
        }

        [Test]
        public async Task UpdateAsync_Throws_IfBasketNotFound()
        {
            var updateDto = new UpdateBasketDto { Id = 101, Items = new List<CreateBasketItemDto>() };

            _basketRepositoryMock.Setup(r => r.GetByIdAsync(updateDto.Id)).ReturnsAsync((Basket)null);

            Assert.ThrowsAsync<KeyNotFoundException>(async () => await _basketService.UpdateAsync(updateDto));
        }

        [Test]
        public async Task UpdateAsync_UpdatesAndReturnsBasketDto()
        {
            var updateDto = new UpdateBasketDto
            {
                Id = 5,
                UserId = Guid.NewGuid(),
                Items = new List<CreateBasketItemDto>
        {
            new CreateBasketItemDto
            {
                ProductId = Guid.NewGuid(),
                Quantity = 2
            }
        }
            };
            var existing = new Basket { Id = 5, Items = new List<BasketItem>() };
            var basketDto = new BasketDto { Id = 5 };

            _basketRepositoryMock.Setup(r => r.GetByIdAsync(updateDto.Id)).ReturnsAsync(existing);
            _basketRepositoryMock.Setup(r => r.UpdateAsync(existing)).Returns(Task.CompletedTask);
            _mapperMock.Setup(m => m.Map<BasketDto>(existing)).Returns(basketDto);

            var result = await _basketService.UpdateAsync(updateDto);

            Assert.IsNotNull(result);
            Assert.AreEqual(5, result.Id);
            _basketRepositoryMock.Verify(r => r.UpdateAsync(existing), Times.Once);
        }

        [Test]
        public async Task ClearBasketAsync_Throws_IfBasketNotFound()
        {
            var userId = Guid.NewGuid();
            _basketRepositoryMock.Setup(r => r.GetByUserIdAsync(userId)).ReturnsAsync((Basket)null);

            Assert.ThrowsAsync<KeyNotFoundException>(async () => await _basketService.ClearBasketAsync(userId));
        }

        [Test]
        public async Task ClearBasketAsync_ClearsBasketItems_IfBasketFound()
        {
            var userId = Guid.NewGuid();
            var basket = new Basket
            {
                Id = 1,
                Items = new List<BasketItem>
                {
                    new BasketItem { ProductId = Guid.NewGuid(), Quantity = new Quantity(1) }
                }
            };

            _basketRepositoryMock.Setup(r => r.GetByUserIdAsync(userId)).ReturnsAsync(basket);
            _basketRepositoryMock.Setup(r => r.UpdateAsync(basket)).Returns(Task.CompletedTask);

            await _basketService.ClearBasketAsync(userId);

            Assert.AreEqual(0, basket.Items.Count);
            _basketRepositoryMock.Verify(r => r.UpdateAsync(basket), Times.Once);
        }

        [Test]
        public async Task DeleteAsync_Throws_IfBasketNotFound()
        {
            _basketRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Basket)null);

            Assert.ThrowsAsync<KeyNotFoundException>(async () => await _basketService.DeleteAsync(17));
        }

        [Test]
        public async Task DeleteAsync_DeletesBasket_IfFound()
        {
            var basket = new Basket { Id = 8 };
            _basketRepositoryMock.Setup(r => r.GetByIdAsync(basket.Id)).ReturnsAsync(basket);
            _basketRepositoryMock.Setup(r => r.DeleteAsync(basket.Id)).Returns(Task.CompletedTask);

            await _basketService.DeleteAsync(basket.Id);

            _basketRepositoryMock.Verify(r => r.DeleteAsync(basket.Id), Times.Once);
        }
    }
}